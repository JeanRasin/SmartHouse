using SmartHouse.Domain.Core.Weather;
using SmartHouse.Infrastructure.Data.Weather.OpenWeather;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestSharpExample
{
    class Program
    {
        static async Task<WeatherResponse> GetWeather()
        {
            var city = "Perm,ru";
            var api = "f4c946ac33b35d68233bbcf83619eb58";

            using (var client = new HttpClient())
            {
                try
                {
                    //https://api.openweathermap.org/data/2.5/weather?q=Perm,ru&APPID=f4c946ac33b35d68233bbcf83619eb58 

                    client.BaseAddress = new Uri("https://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?q={city}&APPID={api}&units=metric");//
                    response.EnsureSuccessStatusCode();

                    string stringResult = await response.Content.ReadAsStringAsync();
                    WeatherResponse rawWeather = JsonSerializer.Deserialize<WeatherResponse>(stringResult);

                    //var result = new WeatherData
                    //{
                    //    Temp = rawWeather.Main.Temp,
                    //    WindSpeed = rawWeather.Wind.Speed,
                    //};

                    return rawWeather;
                }
                //catch (HttpRequestException httpRequestException)
                catch (Exception ex)
                {
                    //return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
            return null;
        }

        static void Main(string[] args)
        {
            var obj = GetWeather().Result;



        }
    }
}
