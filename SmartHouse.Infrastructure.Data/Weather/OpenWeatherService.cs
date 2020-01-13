using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Infrastructure.Data.Weather.OpenWeather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHouse.Infrastructure.Data.Weather
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly string city;
        private readonly string api;

        //public OpenWeatherService(string city, string api)
        public OpenWeatherService(Dictionary<string, string> parm)
        {
            string[] keys = { "city", "api" };
            if (!keys.Any() || !keys.All(key => parm.ContainsKey(key)))
            {
                throw new Exception("Not parameters.");
            }

            city = parm["city"];//"Perm,ru";
            api = parm["api"];// "f4c946ac33b35d68233bbcf83619eb58";

            //this.city = city;//"Perm,ru";
            //this.api = api;// "f4c946ac33b35d68233bbcf83619eb58";
        }

        public async Task<WeatherData> GetWeather()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&APPID={api}&units=metric");
                    response.EnsureSuccessStatusCode();

                    string stringResult = await response.Content.ReadAsStringAsync();
                    WeatherResponse rawWeather = JsonSerializer.Deserialize<WeatherResponse>(stringResult);

                    var result = new WeatherData
                    {
                        Temp = rawWeather.Main.Temp,
                        WindSpeed = rawWeather.Wind.Speed,
                    };

                    return result;
                }
               // catch (HttpRequestException httpRequestException)
                catch (Exception ex)
                {
                    //log write
                }
            }
            return null;
        }
    }
}
