using PioresFilmes.Data.Dto;

namespace PioresFilmes.Data.Interfaces
{
    public interface IProducerRepository
    {
        public Task<IEnumerable<IntervalDto>> FindProducersByConsecutiveWins();
    }
}
