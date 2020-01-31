using SmartHouse.Domain.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Domain.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync(CancellationToken? token = null);
    }
}
