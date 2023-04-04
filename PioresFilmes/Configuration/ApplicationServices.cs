using PioresFilmes.Application.Interfaces;
using PioresFilmes.Application.Services;
using PioresFilmes.Data;
using PioresFilmes.Data.Interfaces;

namespace PioresFilmes.Configuration
{
    internal static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IReadCsvService, ReadCsvService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IInitDatabaseService, InitDatabaseService>();
            services.AddScoped<IInitializeDataService, InitializeDataService>();

            return services;
        }
    }
}
