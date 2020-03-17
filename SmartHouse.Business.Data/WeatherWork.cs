using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public class WeatherWork : IWeatherWork
    {
        private readonly IWeatherService _weatherService;

        public int TimeOutSec { get; }

        /// <summary>
        /// Create a WeatherWork.
        /// </summary>
        /// <param name="weatherService"></param>
        /// <param name="timeOutSec">Waiting time for the weather service.</param>
        public WeatherWork(IWeatherService weatherService, int timeOutSec = 30)
        {
            _weatherService = weatherService;
            TimeOutSec = timeOutSec * 1000;
        }

        /// <summary>
        /// Get weather.
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherModel> GetWeatherAsync()
        {
            var cts = new CancellationTokenSource();
            return await GetWeatherAsync(cts);
        }

        /// <summary>
        /// Get weather with a token.
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
        public async Task<WeatherModel> GetWeatherAsync(CancellationTokenSource tokenSource = default)
        {
            CancellationToken token = tokenSource.Token;

            try
            {
                Task<WeatherModel> task = Task.Run(() =>
               {
                   return _weatherService.GetWeatherAsync(token);
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
