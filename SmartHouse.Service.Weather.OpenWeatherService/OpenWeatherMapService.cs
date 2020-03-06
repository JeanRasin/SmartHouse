using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Service.Weather.OpenWeatherMap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace SmartHouse.Service.Weather.OpenWeatherMap
{
    public class OpenWeatherMapService : IWeatherService, IDisposable
    {
        protected readonly (string url, string city, string api) data;

        protected readonly ILogger<OpenWeatherMapService> logger;
        protected readonly HttpClient client;

        protected readonly string[] keys = { "city", "api", "url" };

        public OpenWeatherMapService(IDictionary<string, string> parm, HttpMessageHandler handler = null, ILogger<OpenWeatherMapService> logger = null)
        {
            parm = new Dictionary<string, string>(parm, StringComparer.OrdinalIgnoreCase);

            if (!keys.All(key => parm.Any(s => s.Key.ToLower() == key)))
            {
                throw new Exception("Not parameters.");
            }

            if (handler == null)
            {
                client = new HttpClient();
            }
            else
            {
                client = new HttpClient(handler);
            }

            data = (parm["url"], parm["city"], parm["api"]);

            client.BaseAddress = new Uri(data.url);
            client.Timeout = TimeSpan.FromMilliseconds(1000);

            this.logger = logger;
        }

        public async Task<WeatherModel> GetWeatherAsync()
        {
            return await GetWeatherAsync(null);
        }

        public async Task<WeatherModel> GetWeatherAsync(CancellationToken? token)
        {
            var IsCancellationRequested = token.GetValueOrDefault().IsCancellationRequested;
            if (IsCancellationRequested == true)
            {
                throw new OperationCanceledException("Operation aborted by token.");
            }

            try
            {
                var response = await client.GetAsync($"/data/2.5/weather?q={data.city}&APPID={data.api}&units=metric");
                response.EnsureSuccessStatusCode();

                string requestJson = JsonSerializer.Serialize(response.RequestMessage);
                LogInfoWrite(requestJson);

                string stringResult = await response.Content.ReadAsStringAsync();
                WeatherResponse rawWeather = JsonSerializer.Deserialize<WeatherResponse>(stringResult);

                var result = new WeatherModel
                {
                    Temp = rawWeather.Main.Temp,
                    WindSpeed = (int)rawWeather.Wind.Speed,
                    WindDeg = rawWeather.Wind.Deg,
                    City = rawWeather.Name,
                    FeelsLike = rawWeather.Main.FeelsLike,
                    Pressure = rawWeather.Main.Pressure,
                    Humidity = rawWeather.Main.Humidity
                };

                result.Description = rawWeather.Weather.Count() > 0 ? string.Join("; ", rawWeather.Weather.Select(s => s.Description)) : string.Empty;

                LogInfoWrite(stringResult);

                return result;
            }
            catch (HttpRequestException ex)
            {
                LogErrorWrite(ex);

                if (token != null)
                {
                    throw ex;
                }

                await GetWeatherAsync(token);
            }
            catch (OperationCanceledException ex)
            {
                LogErrorWrite(ex);
                throw new InvalidOperationException("Expected timeout exception");
            }
            catch (Exception ex)
            {
                LogErrorWrite(ex);
                throw ex;
            }

            return null;
        }

        private void LogErrorWrite(Exception ex)
        {
            if (logger != null)
                logger.LogError(new EventId(4), ex, ex.Message);
        }

        private void LogInfoWrite(string message)
        {
            if (logger != null)
                logger.LogInformation(new EventId(5), message);
        }

        #region dispose
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    client.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
