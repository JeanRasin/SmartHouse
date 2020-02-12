using SmartHouse.Domain.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public interface IWeatherWork
    {
        public Task<WeatherModel> GetWeatherAsync();
        public Task<WeatherModel> GetWeatherAsync(CancellationTokenSource tokenSource = default);
    }
}
