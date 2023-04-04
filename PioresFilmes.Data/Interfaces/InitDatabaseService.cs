using Dapper;
using System.Data;

namespace PioresFilmes.Data.Interfaces
{
    public class InitDatabaseService : IInitDatabaseService
    {
        private readonly IDbConnection _dbConnection;

        public InitDatabaseService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void InitDatabase()
        {
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
    }
}
