using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Infrastructure.Data.Weather.OpenWeather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHouse.Infrastructure.Data.Weather
{
    public class OpenWeatherService : IWeatherService, IDisposable
    {
        private readonly string city;
        private readonly string api;
        private readonly ILogger<OpenWeatherService> logger;
        private readonly HttpClient client;

        public OpenWeatherService(ILogger<OpenWeatherService> logger, Dictionary<string, string> parm, HttpMessageHandler handler = null)
        {
            string[] keys = { "city", "api" };
            if (!keys.All(key => parm.ContainsKey(key)))
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

            client.Timeout = TimeSpan.FromMilliseconds(1000);

            city = parm["city"];
            api = parm["api"];

            this.logger = logger;
        }

        public OpenWeatherService(Dictionary<string, string> parm, HttpMessageHandler handler = null) : this(null, parm, handler)
        {

        }


        public async Task<WeatherData> GetWeather()
        {
            try
            {
                client.BaseAddress = new Uri("https://api.openweathermap.org");
                var response = await client.GetAsync($"/data/2.5/weather?q={city}&APPID={api}&units=metric");
                response.EnsureSuccessStatusCode();

                string requestJson = JsonSerializer.Serialize(response.RequestMessage);
                LogInfoWrite(1, requestJson);

                string stringResult = await response.Content.ReadAsStringAsync();
                WeatherResponse rawWeather = JsonSerializer.Deserialize<WeatherResponse>(stringResult);

                var result = new WeatherData
                {
                    Temp = rawWeather.Main.Temp,
                    WindSpeed = rawWeather.Wind.Speed,
                };

                LogInfoWrite(2, stringResult);

                return result;
            }
            catch (HttpRequestException ex)
            {
                LogErrorWrite(3, ex);
                throw ex;
            }
            catch (JsonException ex)
            {
                LogErrorWrite(3, ex);
                throw ex;
            }
            catch (OperationCanceledException ex)
            {
                LogErrorWrite(3, ex);
                throw new InvalidOperationException("Expected timeout exception");
            }
            catch (Exception ex)
            {
                LogErrorWrite(3, ex);
                throw ex;
            }
        }

        private void LogErrorWrite(int eventId, Exception ex)
        {
            if (logger != null)
                logger.LogError(new EventId(eventId), ex, ex.Message);
        }

        private void LogInfoWrite(int eventId, string message)
        {
            if (logger != null)
                logger.LogInformation(new EventId(eventId), message);
        }

        #region dispose
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    client.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
