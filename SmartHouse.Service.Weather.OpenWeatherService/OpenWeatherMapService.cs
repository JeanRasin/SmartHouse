using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Service.Weather.OpenWeatherMap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHouse.Service.Weather.OpenWeatherMap
{
    public class OpenWeatherMapService : IWeatherService, IDisposable
    {
        private readonly string city;
        private readonly string api;
        private readonly ILogger<OpenWeatherMapService> logger;
        private readonly HttpClient client;

        private readonly string url;// = "https://api.openweathermap.org";

        public OpenWeatherMapService(ILogger<OpenWeatherMapService> logger, Dictionary<string, string> parm, HttpMessageHandler handler = null)
        {
            string[] keys = { "city", "api", "url" };
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

            url = parm["url"];
            city = parm["city"];
            api = parm["api"];

            this.logger = logger;
        }

        public OpenWeatherMapService(Dictionary<string, string> parm, HttpMessageHandler handler = null) : this(null, parm, handler)
        {

        }


        public async Task<WeatherData> GetWeatherAsync()
        {
            try
            {
                client.BaseAddress = new Uri(url);
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
