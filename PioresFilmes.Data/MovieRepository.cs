using Dapper;
using Microsoft.Extensions.Logging;
using PioresFilmes.Data.Interfaces;
using PioresFilmes.Domain;
using System.Data;

namespace PioresFilmes.Data
{
    public class MovieRepository: IMovieRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovieRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task CreateManyAsync(IEnumerable<Movie> movies)
        {
            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                var producers = movies.SelectMany(m => m.Producers)
                                      .DistinctBy(p => p.Name);
                await _dbConnection.ExecuteAsync("INSERT INTO Producers (Name) VALUES (@Name);", producers, transaction);

                foreach (var movie in movies)
                {
                    var movieId = await _dbConnection.ExecuteScalarAsync<long>("INSERT INTO Movies (Title, Year, Studios, Winner) VALUES (@Title, @Year, @Studios, @Winner); SELECT last_insert_rowid()", movie, transaction);

                    foreach (var producer in movie.Producers)
                    {
                        var existingProducerId = await _dbConnection.ExecuteScalarAsync<long>("SELECT Id FROM Producers WHERE Name = @Name", new { Name = producer.Name }, transaction);
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
    }
}
