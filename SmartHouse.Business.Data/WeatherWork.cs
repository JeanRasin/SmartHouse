using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public class WeatherWork
    {
        public int TimeOutSec { get; }

        private readonly IWeatherService weatherService;

        public WeatherWork(IWeatherService weatherService, int timeOutSec = 30)
        {
            this.weatherService = weatherService;
            TimeOutSec = timeOutSec * 1000;
        }

        public async Task<WeatherModel> GetWeatherAsync()
        {
            var cts = new CancellationTokenSource();
            return await GetWeatherAsync(cts);
        }

        public async Task<WeatherModel> GetWeatherAsync(CancellationTokenSource tokenSource = default)
        {
            CancellationToken token = tokenSource.Token;

            try
            {
                Task<WeatherModel> task = Task.Run(() =>
               {
                   return weatherService.GetWeatherAsync(token);
               });

                Task.WaitAll(new[] { task }, TimeOutSec);
                tokenSource.Cancel();
                return await task;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Timed out.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
