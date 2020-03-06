using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Service.Weather.OpenWeatherMap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace SmartHouse.Service.Weather.OpenWeatherMap
{
    public class OpenWeatherMapService : IWeatherService, IDisposable
    {
        const int TimeOutMilSec = 1000;

        protected readonly (string url, string city, string api) data;
        protected readonly ILogger<OpenWeatherMapService> logger;
        protected readonly HttpClient httpClient;
        protected readonly Mapper mapper;
        protected readonly string[] keys = { "city", "api", "url" };

        public OpenWeatherMapService(IDictionary<string, string> parm, HttpClient httpClient, ILogger<OpenWeatherMapService> logger = null)
        {
            parm = new Dictionary<string, string>(parm, StringComparer.OrdinalIgnoreCase);

            if (!keys.All(key => parm.Any(s => s.Key.ToLower() == key)))
            {
                throw new Exception("Not parameters.");
            }

            this.httpClient = httpClient;
            
            data = (parm["url"], parm["city"], parm["api"]);

            this.httpClient.BaseAddress = new Uri(data.url);
            this.httpClient.Timeout = TimeSpan.FromMilliseconds(TimeOutMilSec);

            this.logger = logger;

            // Config AutoMapper.
            var config = new MapperConfiguration(cfg => cfg.CreateMap<WeatherResponse, WeatherModel>()
                .ForMember("Temp", opt => opt.MapFrom(src => src.Main.Temp))
                .ForMember("WindSpeed", opt => opt.MapFrom(src => (int)src.Wind.Speed))
                .ForMember("WindDeg", opt => opt.MapFrom(src => src.Wind.Deg))
                .ForMember("City", opt => opt.MapFrom(src => src.Name))
                .ForMember("FeelsLike", opt => opt.MapFrom(src => src.Main.FeelsLike))
                .ForMember("Pressure", opt => opt.MapFrom(src => src.Main.Pressure))
                .ForMember("Humidity", opt => opt.MapFrom(src => src.Main.Humidity))
                .ForMember("Description", opt => opt.MapFrom(src => src.Weather.Count() > 0 ? string.Join("; ", src.Weather.Select(s => s.Description)) : string.Empty)));

            mapper = new Mapper(config);
        }

        public async Task<WeatherModel> GetWeatherAsync()
        {
            return await GetWeatherAsync(null);
        }

        public async Task<WeatherModel> GetWeatherAsync(CancellationToken? token)
        {
            //var IsCancellationRequested = token.GetValueOrDefault().IsCancellationRequested;
            //if (IsCancellationRequested == true)
            //{
            //    throw new OperationCanceledException("Operation aborted by token.");
            //}

            try
            {
                var b = true;
                HttpResponseMessage response = null;
                while (b)
                {
                     response = await httpClient.GetAsync($"/data/2.5/weather?q={data.city}&APPID={data.api}&units=metric");

                    HttpRequestException ex;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            ex = new HttpRequestException("401");
                            LogErrorWrite(ex);
                            throw ex;
                        case HttpStatusCode.ServiceUnavailable:
                            ex = new HttpRequestException("503");
                            LogErrorWrite(ex);
                            if (token == null)
                            {
                                throw ex;
                                // await GetWeatherAsync(token);
                            }
                            
                            break;
                        default:
                            b = false;
                            break;
                    }

                    var IsCancellationRequested = token.GetValueOrDefault().IsCancellationRequested;
                    if (IsCancellationRequested == true)
                    {
                       //var invalidOperationEx = new InvalidOperationException("Expected timeout exception");
                       // LogErrorWrite(invalidOperationEx);
                       // throw invalidOperationEx;
                         throw new OperationCanceledException("Operation aborted by token.");
                    }

                   // response.EnsureSuccessStatusCode();
                }

                string requestJson = JsonSerializer.Serialize(response.RequestMessage);
                LogInfoWrite(requestJson);

                string stringResult = await response.Content.ReadAsStringAsync();
                WeatherResponse rawWeather = JsonSerializer.Deserialize<WeatherResponse>(stringResult);

                WeatherModel result = mapper.Map<WeatherResponse, WeatherModel>(rawWeather);

                LogInfoWrite(stringResult);

                return result;
            }
            //catch (HttpRequestException ex)
            //{
            //    LogErrorWrite(ex);

            //    if (token == null)
            //    {
            //        throw ex;
            //    }

            //    await GetWeatherAsync(token);
            //}
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

           // return null;
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
                    httpClient.Dispose();
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
