using Newtonsoft.Json;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public class WeatherWork : IWeatherWork
    {
        private readonly IWeatherService _weatherService;
        private readonly IDatabase _database;
        private readonly TimeSpan _cacheTime;

        public int TimeOutSec { get; }

        /// <summary>
        /// Create a WeatherWork.
        /// </summary>
        /// <param name="weatherService"></param>
        /// <param name="database">Redis database.</param>
        /// <param name="cacheTime">Cache recording time.</param>
        /// <param name="timeOutSec">Waiting time for the weather service.</param>
        public WeatherWork(IWeatherService weatherService, IDatabase database,
            TimeSpan cacheTime, int timeOutSec = 30)
        {
            _weatherService = weatherService;
            _database = database;
            _cacheTime = cacheTime;
            TimeOutSec = timeOutSec * 1000;
        }

        /// <summary>
        /// Get weather.
        /// </summary>
        /// <returns></returns>
        public async Task<Weather> GetWeatherAsync()
        {
            var cts = new CancellationTokenSource();
            return await GetWeatherAsync(cts);
        }

        /// <summary>
        /// Get weather with a token.
        /// </summary>
        /// <param name="tokenSource"></param>
        /// <returns></returns>
        public async Task<Weather> GetWeatherAsync(CancellationTokenSource tokenSource = default)
        {
            CancellationToken token = tokenSource.Token;
            Weather result;

            try
            {
                RedisValue weather = await _database.StringGetAsync("GetWeather");

                if (weather.IsNull)
                {
                    Task<Weather> task = Task.Run(() =>
                   {
                       return _weatherService.GetWeatherAsync(token);
                   });

                    Task.WaitAll(new[] { task }, TimeOutSec);

                    tokenSource.Cancel();

                    result = await task;

                    string weatherJson = JsonConvert.SerializeObject(result);
                    await _database
                        .StringSetAsync("GetWeather",
                        new RedisValue(weatherJson),
                        _cacheTime);

                    return result;
                }
                else
                {
                    result = JsonConvert.DeserializeObject<Weather>(weather);
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Timed out.");
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}