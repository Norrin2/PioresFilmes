using PioresFilmes.Data.Dto;
using PioresFilmes.Domain;

namespace PioresFilmes.Data.Interfaces
{
    public interface IProducerRepository
    {
        public Task<IntervalDto> FindProducerWithLeastIntervalBetweenWins();
        public Task<IntervalDto> FindProducerWithGreatestIntervalBetweenWins();
    }
}
