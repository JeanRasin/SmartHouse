using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
       // private readonly IWeatherService weatherService;
        private readonly WeatherWork weatherWork;

        public WeatherController(IWeatherService ws)
        {
           // weatherService = ws;
            weatherWork = new WeatherWork(ws, 100);
        }

        [HttpGet("get")]
        public WeatherData GetWeather()
        {
            var result = weatherWork.GetWeather();

            return result;
        }

    }
}