using PioresFilmes.Application.Dto;
using PioresFilmes.Application.Interfaces;
using PioresFilmes.Data.Interfaces;

namespace PioresFilmes.Application.Services
{
    public class FindProducersService: IFindProducersService
    {
        public readonly IProducerRepository _producerRepository;

        public FindProducersService(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }

        public async Task<FindProducersDto> FindProducerIntervalsAsync()
        {

            var min = _producerRepository.FindProducerWithLeastIntervalBetweenWins();
            var max = _producerRepository.FindProducerWithGreatestIntervalBetweenWins();

            await Task.WhenAll(min, max);
            return new FindProducersDto() { Max = max.Result, Min = min.Result };
        }
    }
}
