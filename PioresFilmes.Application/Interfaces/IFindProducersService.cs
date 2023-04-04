using PioresFilmes.Application.Dto;

namespace PioresFilmes.Application.Interfaces
{
    public interface IFindProducersService
    {
        public Task<FindProducersDto> FindProducerIntervalsAsync();
    }
}
