using SmartHouse.Domain.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Services.Interfaces
{
    public interface IWeatherWork
    {
        public Task<Weather> GetWeatherAsync();

        public Task<Weather> GetWeatherAsync(CancellationTokenSource tokenSource = default);
    }
}