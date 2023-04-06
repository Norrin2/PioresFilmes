using PioresFilmes.Data.Dto;

namespace PioresFilmes.Application.Dto
{
    public record FindProducersDto
    {
        public IEnumerable<IntervalDto> Min { get; set; }
        public IEnumerable<IntervalDto> Max { get; set; }
    }
}
