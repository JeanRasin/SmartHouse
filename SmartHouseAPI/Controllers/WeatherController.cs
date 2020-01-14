using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService weatherService;

        public WeatherController(IWeatherService ws)
        {
            weatherService = ws;
        }

        [HttpGet("get")]
        public WeatherData GetWeather()
        {
            var result = weatherService.GetWeather().Result;

            return result;
        }

    }
}