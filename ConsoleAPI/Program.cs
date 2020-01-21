using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Service.Weather.Gismeteo;
using SmartHouse.Service.Weather.OpenWeatherMap;
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
                { "url", "https://api.openweathermap.org" },
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58" }
            };
            
            var loggerContext = new LoggerContext("mongodb://localhost:27017", "smartHouseLogger");

            var serviceProvider = new ServiceCollection()
            .AddLogging()
              //.AddLogging(cfg => cfg.Services.AddSingleton<ILogger>(x => {
              //    // x.GetService<ILoggerFactory>().AddContext(loggerContext);
              //    return new LoggerWork(loggerContext);
              //}))
              //  .AddLogging(cfg => cfg.Services.AddSingleton<ILogger>(x => new LoggerWork(loggerContext)))
              //.AddSingleton<ILogger>(x=> new LoggerWork(loggerContext)) // GisMeteo service.
              .AddSingleton<IWeatherService>(x => new OpenWeatherMapService(x.GetRequiredService<ILogger<OpenWeatherMapService>>(), parm)) // OpenWeatherMap service.
                                                                                                                                           //.AddSingleton<IWeatherService, GisMeteoService>() // GisMeteo service.

           .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddContext(loggerContext);
          //  var logger = loggerFactory.CreateLogger("NewLogger");

            var connectStr = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres";
            var postgreContext = new GoalContext(connectStr);
            var goalWork = new GoalWork(postgreContext);

            List<GoalModel> items = goalWork.GetGoals();

            string jsonStr = JsonSerializer.Serialize(items);
            Console.WriteLine(jsonStr);

            //

          //  var logger = serviceProvider.GetService<ILogger>();

            // var log = new LoggerWork(loggerContext);

            //LoggerWork lw = (LoggerWork)logger;

          //  logger.LogInformation("test_log_write");

            //var logItems = lw.GetLogger();
            //var logJson = JsonSerializer.Serialize(logItems);
            //Console.WriteLine($"Log: {logJson}");

            //

            var ows = serviceProvider.GetService<IWeatherService>();

            var weatherWork = new WeatherWork(ows);

            var weather = weatherWork.GetWeather();
            var weatherJson = JsonSerializer.Serialize(weather);

            Console.WriteLine($"Weather: {weatherJson}");


            Console.WriteLine("Exit");
        }
    }
}
