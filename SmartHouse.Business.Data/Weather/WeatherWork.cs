using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using System;
using System.Net.Http;

namespace SmartHouse.Business.Data.Weather
{
    public class WeatherWork
    {
        public int CounterMax { get; }

        private readonly IWeatherService weatherService;
        private int counter = 0;
        private int hour = int.MaxValue;

        //        public WeatherWork(IWeatherService weatherService, int counterMax)
        public WeatherWork(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
           //CounterMax = counterMax;
        }

        public WeatherData GetWeather()
        {
            if (DateTime.Now.Hour < hour)
            {
                hour = DateTime.Now.Hour;
                counter = 0;
            }

            if (counter > CounterMax)
            {
                throw new Exception("The number of requests per day exceeded.");
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

                //if (ex.Message == "Response status code does not indicate success: 503 (Service Unavailable).") //todo:!!!
                //{
                //    System.Threading.Thread.Sleep(2000);
                //    result = GetWeather();
                //}

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
