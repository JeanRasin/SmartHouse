using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data.Weather
{
    public class WeatherWork
    {
        private const int counterMax = 2;

        private readonly IWeatherService weatherService;
        private int counter = 0;
        private int hour = 0;

        public WeatherWork(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        //public async Task<WeatherData> GetWeather()
        public WeatherData GetWeather()
        {
            if(DateTime.Now.Hour < hour)
            {
                hour = DateTime.Now.Hour;
                counter = 0;
            }

            if (counter >= counterMax)
            {
                throw new Exception("!!!");
            }

            WeatherData result;
            try
            {
                result = weatherService.GetWeather().Result;
                counter++;

            }
            catch (HttpRequestException ex)
            {
                //if (ex.InnerException is WebException webException && webException.Status == WebExceptionStatus.NameResolutionFailure)
                //{
                //    // return true;
                //}

                if (ex.Message == "Response status code does not indicate success: 503 (Service Unavailable).")
                {
                    Task.Delay(2000);
                    result = GetWeather();
                }

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
