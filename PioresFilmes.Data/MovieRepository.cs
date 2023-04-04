using Dapper;
using Microsoft.Data.Sqlite;
using PioresFilmes.Data.Interfaces;
using PioresFilmes.Domain;

namespace PioresFilmes.Data
{
    public class MovieRepository: IMovieRepository
    {
        private readonly SqliteConnection _dbConnection;

        public MovieRepository()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:;Cache=Shared");

            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                _dbConnection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Movies (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Year INTEGER NOT NULL,
                        Studios TEXT NOT NULL,
                        Winner BOOLEAN NOT NULL
                    );
                ");
                _dbConnection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Producers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );
                ");
                _dbConnection.Execute(@"
                    CREATE TABLE IF NOT EXISTS MovieProducers (
                        MovieId INTEGER NOT NULL,
                        ProducerId INTEGER NOT NULL,
                        PRIMARY KEY (MovieId, ProducerId),
                        FOREIGN KEY (MovieId) REFERENCES Movies(Id),
                        FOREIGN KEY (ProducerId) REFERENCES Producers(Id)
                    );
                ");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task CreateManyAsync(IEnumerable<Movie> movies)
        {
            await _dbConnection.OpenAsync();
            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                foreach (var movie in movies)
                {
                    var movieId = await _dbConnection.ExecuteScalarAsync<long>("INSERT INTO Movies (Title, Year, Studios, Winner) VALUES (@Title, @Year, @Studios, @Winner); SELECT last_insert_rowid()", movie, transaction);

                    foreach (var producer in movie.Producers)
                    {
                        var existingProducerId = await _dbConnection.ExecuteScalarAsync<long>("SELECT Id FROM Producers WHERE Name = @Name", new { Name = producer.Name }, transaction);
                        if (existingProducerId == 0)
                        {
                            existingProducerId = await _dbConnection.ExecuteScalarAsync<long>("INSERT INTO Producers (Name) VALUES (@Name);  SELECT last_insert_rowid()", producer, transaction);
                        }

                        await _dbConnection.ExecuteAsync("INSERT INTO MovieProducers (MovieId, ProducerId) VALUES (@MovieId, @ProducerId)", new { MovieId = movieId, ProducerId = existingProducerId }, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Producer> FindProducerWithLeastInterval()
        {
            await _dbConnection.OpenAsync();
            var result = _dbConnection.QueryFirstOrDefault<Producer>(@"
            SELECT
                p.Id,
                p.Name
            FROM
                Producer p
                INNER JOIN MovieProducer prev_mp ON prev_mp.ProducerId = p.Id
                INNER JOIN Movie prev ON prev_mp.MovieId = prev.Id AND prev.Winner = 1
                INNER JOIN MovieProducer curr_mp ON curr_mp.ProducerId = p.Id
                INNER JOIN Movie curr ON curr_mp.MovieId = curr.Id AND curr.Winner = 1 AND curr.Year > prev.Year
            GROUP BY
                p.Id,
                p.Name
            HAVING
                COUNT(*) > 1
            ORDER BY
                MIN(curr.Year - prev.Year)
            LIMIT
                1");
                return result;
        }
    }
}
