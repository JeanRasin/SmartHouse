using SmartHouse.Domain.Core.Weather;
using System.Threading.Tasks;

namespace SmartHouse.Domain.Interfaces.Weather
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync();
    }
}
