using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public class WeatherWork
    {
        public int CounterMax { get; }

        private readonly IWeatherService weatherService;
        //private int counter = 0;
        //private int hour = int.MaxValue;

        //        public WeatherWork(IWeatherService weatherService, int counterMax)
        public WeatherWork(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
           //CounterMax = counterMax;
        }

        public async Task<WeatherModel> GetWeatherAsync()
        {
            //if (DateTime.Now.Hour < hour)
            //{
            //    hour = DateTime.Now.Hour;
            //    counter = 0;
            //}

            //if (counter > CounterMax)
            //{
            //    throw new Exception("The number of requests per day exceeded.");
            //}

            WeatherModel result;
            try
            {
                result = await weatherService.GetWeatherAsync();
               // counter++;

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
