﻿using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using System.Threading.Tasks;

namespace SmartHouse.Service.Weather.Gismeteo
{
    public class GisMeteoService : IWeatherService
    {
        public async Task<WeatherData> GetWeatherAsync()
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(2000);

                // Test data.
                return new WeatherData(temp: 100, windSpeed: 15);
            });
        }
    }
}
