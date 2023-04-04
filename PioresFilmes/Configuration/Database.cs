using Microsoft.Data.Sqlite;
using System.Data;

namespace PioresFilmes.Configuration
{
    internal static class Database
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var connection = new SqliteConnection("Data Source=:memory:;Cache=Shared");
            connection.Open();
            services.AddScoped<IDbConnection>((x) => connection);
            return services;
        }
    }
}
