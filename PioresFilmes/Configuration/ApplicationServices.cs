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
            services.AddSingleton<IReadCsvService, ReadCsvService>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IProducerRepository, ProducerRepository>();
            services.AddSingleton<IFindProducersService, FindProducersService>();
            services.AddSingleton<IInitDatabaseService, InitDatabaseService>();
            services.AddSingleton<IInitializeDataService, InitializeDataService>();

            return services;
        }
    }
}
