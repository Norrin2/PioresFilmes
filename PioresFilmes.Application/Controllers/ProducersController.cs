using Microsoft.AspNetCore.Mvc;
using PioresFilmes.Application.Dto;
using PioresFilmes.Application.Interfaces;

namespace PioresFilmes.Application.Controllers
{
    public class FindProducersController : ControllerBase
    {
        private readonly IFindProducersService _findProducersService;

        public FindProducersController(IFindProducersService findProducersService)
        {
            _findProducersService = findProducersService;
        }

        [HttpGet("/producers/intervals")]
        public async Task<ActionResult<FindProducersDto>> FindProducerIntervalsAsync()
        {
            var result = await _findProducersService.FindProducerIntervalsAsync();
            return Ok(result);
        }
    }
}
