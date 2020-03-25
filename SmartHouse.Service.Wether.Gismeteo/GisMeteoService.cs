using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Service.Weather.Gismeteo
{
    public class GisMeteoService : IWeatherService
    {
        public async Task<WeatherModel> GetWeatherAsync(CancellationToken? token = null)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(2000);

                // Test data.
                return new WeatherModel(temp: 100, windSpeed: 15, windDeg: 150, city: "Perm", pressure: 10, humidity: 14, description: "Snow", feelsLike: 0);
            });
        }
    }
}