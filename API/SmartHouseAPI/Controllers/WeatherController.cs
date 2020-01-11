using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core;
using SmartHouse.Services.Interfaces;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherWork _weatherWork;

        public WeatherController(IWeatherWork weatherWork)
        {
            _weatherWork = weatherWork;
        }

        // GET api/weather
        [HttpGet]
        [Produces("application/json")]
        [ResponseCache(CacheProfileName = "WeatherCaching")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeatherAsync()
        {
            Weather result = await _weatherWork.GetWeatherAsync();

            if (result == null)
            {
                return NotFound("Weather result is null.");
            }

            return Ok(result);
        }
    }
}