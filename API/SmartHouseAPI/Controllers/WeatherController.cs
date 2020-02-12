using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public WeatherController(IWeatherWork weatherWork)
        {
            this.weatherWork = weatherWork;
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
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }
    }
}