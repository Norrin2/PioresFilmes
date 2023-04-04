using PioresFilmes.Domain;

namespace PioresFilmes.Data.Interfaces
{
    public interface IProducerRepository
    {
        public Task<Producer> FindProducerWithLeastIntervalBetweenWins();
        public Task<Producer> FindProducerWithGreatestIntervalBetweenWins();
    }
}
