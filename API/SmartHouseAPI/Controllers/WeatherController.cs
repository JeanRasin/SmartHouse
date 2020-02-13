using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using System;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherWork weatherWork;
        private readonly ILogger log;

        public WeatherController(IWeatherWork weatherWork, ILogger log)
        {
            this.weatherWork = weatherWork;
            this.log = log;
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
            try
            {
                WeatherModel result = await weatherWork.GetWeatherAsync();

                if (result == null)
                {
                    log.LogError("Weather result null.");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}