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

            var producersByWins = await _producerRepository.FindProducersByConsecutiveWins();

            var min = producersByWins.GroupBy(r => r.Interval)
                                     .OrderBy(g => g.Key)
                                     .First()
                                     .ToList()
                                     .OrderBy(producer => producer.Name);

            var max = producersByWins.GroupBy(r => r.Interval)
                                     .OrderByDescending(g => g.Key)
                                     .First()
                                     .ToList()
                                     .OrderBy(producer => producer.Name);

            return new FindProducersDto() { Max = max, Min = min };
        }
    }
}
