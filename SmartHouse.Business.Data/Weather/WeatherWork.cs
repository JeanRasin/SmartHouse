using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data.Weather
{
    public class WeatherWork
    {
        private readonly IWeatherService weatherService;
        public WeatherWork(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        public async Task<WeatherData> GetWeather()
        {
            return await weatherService.GetWeather();
        }
    }
}
