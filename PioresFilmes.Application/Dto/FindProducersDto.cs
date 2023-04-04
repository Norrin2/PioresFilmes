using PioresFilmes.Data.Dto;

namespace PioresFilmes.Application.Dto
{
    public record FindProducersDto
    {
        public IntervalDto Min { get; set; }
        public IntervalDto Max { get; set; }
    }
}
