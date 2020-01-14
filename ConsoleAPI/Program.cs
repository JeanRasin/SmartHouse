using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Weather;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleAPI
{
    /*
    public interface ITestService
    {
        void Run();
    }

    class TestService : ITestService
    {
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger)
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogWarning("Wow! We are now in the test service.");
        }
    }

    public class App
    {
        private readonly ITestService _testService;
        private readonly ILogger<App> _logger;

        public App(ITestService testService,
            ILogger<App> logger)
        {
            _testService = testService;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation($"This is a console application for {_config.Title}");
            _testService.Run();
            System.Console.ReadKey();
        }
    }

    public class AppSettings
    {
        public string Title { get; set; }
    }
    */
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            */

            var parm = new Dictionary<string, string>
            {
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58w" }
            };

            var serviceProvider = new ServiceCollection()
           .AddLogging(cfg => cfg.AddConsole())
           .AddSingleton<IWeatherService>(x => new OpenWeatherService(x.GetRequiredService<ILogger<OpenWeatherService>>(), parm))
            //.AddSingleton<IWeatherService, OpenWeatherService>()
             //.AddSingleton<IWeatherService, BarService>()
            .BuildServiceProvider();

            var context = new ApplicationContext();
            var goalWork = new GoalWork(context);

            List<Goal> items = goalWork.GetGoals();

            string jsonStr = JsonSerializer.Serialize(items);
            Console.WriteLine(jsonStr);


            //



            // var ows = new OpenWeatherService(parm);

            var ows = serviceProvider.GetService<IWeatherService>();
            var logger = serviceProvider.GetService<ILogger>();

            var weatherWork = new WeatherWork(ows);

            var weather = weatherWork.GetWeather().Result;
            var weatherJson = JsonSerializer.Serialize(weather);

            Console.WriteLine($"Weather: {weatherJson}");


            Console.WriteLine("Exit");
        }
    }
}
