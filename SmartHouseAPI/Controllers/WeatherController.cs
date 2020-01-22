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
        private readonly WeatherWork weatherWork;

        public WeatherController(IWeatherService ws)
        {
            weatherWork = new WeatherWork(ws);
        }

        [HttpGet("get")]
        public WeatherModel GetWeather()
        {
            var result = weatherWork.GetWeather();

            return result;
        }

    }
}