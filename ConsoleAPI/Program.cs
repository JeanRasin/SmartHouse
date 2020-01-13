using Microsoft.EntityFrameworkCore;
using SmartHouse.Business.Data;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Weather;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ConsoleAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ApplicationContext();
            var goalWork = new GoalWork(context);

            List<Goal> items = goalWork.GetGoals();

            string jsonStr = JsonSerializer.Serialize(items);
            Console.WriteLine(jsonStr);


            //

            var parm = new Dictionary<string, string>
            {
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58" }
            };

            var ows = new OpenWeatherService(parm);
            var weatherWork = new WeatherWork(ows);

            var weather = weatherWork.GetWeather().Result;
            var weatherJson = JsonSerializer.Serialize(weather);

            Console.WriteLine($"Weather: {weatherJson}");


            Console.WriteLine("Exit");
        }
    }
}
