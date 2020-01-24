using SmartHouse.Domain.Core;
using System.Threading.Tasks;

namespace SmartHouse.Domain.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync();
    }
}
