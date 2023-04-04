using Microsoft.Extensions.Logging;
using PioresFilmes.Application.Interfaces;
using PioresFilmes.Data.Interfaces;

namespace PioresFilmes.Application.Services
{
    public class InitializeDataService : IInitializeDataService
    {
        private readonly IInitDatabaseService _initDatabaseService;
        private readonly IReadCsvService _readCsvService;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<InitializeDataService> _logger;
        private static readonly string CSV_FILENAME = "movielist.csv";

        public InitializeDataService(IReadCsvService readCsvService,
                                     IMovieRepository movieRepository,
                                     IInitDatabaseService initDatabaseService,
                                     ILogger<InitializeDataService> logger)
        {
            _readCsvService = readCsvService;
            _movieRepository = movieRepository;
            _initDatabaseService = initDatabaseService;
            _logger = logger;
        }

        public async Task Initialize()
        {
            try
            {
                _initDatabaseService.InitDatabase();
                var movies = _readCsvService.ReadMovies(CSV_FILENAME);
                await _movieRepository.CreateManyAsync(movies);
            } catch (FileNotFoundException ex)
            {
                _logger.LogError("Csv file was not found");
                _logger.LogError(ex.Message);
            }
        }
    }
}
