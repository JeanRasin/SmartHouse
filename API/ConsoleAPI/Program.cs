using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Service.Weather.Gismeteo;
using SmartHouse.Service.Weather.OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ConsoleAPI.Helpers;
using SmartHouse.Domain.Interfaces;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAPI
{
    class Program
    {
        public static IWeatherService WeatherService { get; set; }
        public static WeatherWork Weather { get; set; }

        public static GoalContext PostgreContext { get; set; }
        public static GoalWork Goal { get; set; }

        public static LoggerWork Logger { get; set; }

        static async Task Main(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            //var weatherCachingTime = configuration["WeatherCachingTime"];

            IDictionary<string, string> parm = configuration.GetSection("OpenWeatherMapService").Get<OpenWeatherMapServiceConfig>().ToDictionary<string>();

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



            Console.WriteLine("Hello.");

            string command;
            bool quitNow = false;
            while (!quitNow)
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "/help":
                        Console.WriteLine("This should be help.");
                        break;

                    case "/version":
                        Console.WriteLine("This should be version.");
                        break;

                    case "/quit":
                        quitNow = true;
                        break;

                    case "/q":
                        quitNow = true;
                        break;

                    case "/w":

                        if (WeatherService == null)
                        {
                            WeatherService = serviceProvider.GetService<IWeatherService>();
                        }

                        if (Weather == null)
                        {
                            Weather = new WeatherWork(WeatherService);
                        }

                        var weather = await Weather.GetWeatherAsync();
                        var weatherJson = JsonSerializer.Serialize(weather);

                        Console.WriteLine($"Weather: {weatherJson}");

                        break;

                    case "/g":
                        if (PostgreContext == null)
                        {
                            string connectStr = configuration.GetConnectionString("DefaultConnection");
                            PostgreContext = new GoalContext(connectStr);
                        }

                        if (Goal == null)
                        {
                            Goal = new GoalWork(PostgreContext);
                        }

                        List<GoalModel> goalItems = Goal.GetGoals();

                        string goalJson = JsonSerializer.Serialize(goalItems);
                        Console.WriteLine(goalJson);
                        break;

                    case "/lw":

                        if (Logger == null)
                        {
                            MongoDbLoggerConnectionConfig logConfig = configuration.GetSection("MongoDbLoggerConnection").Get<MongoDbLoggerConnectionConfig>();
                            var loggerContext = new LoggerContext(logConfig.Connection, logConfig.DbName);

                            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                            loggerFactory.AddContext(loggerContext);
                            //  var logger = loggerFactory.CreateLogger("NewLogger");

                            var loggerService = serviceProvider.GetService<ILogger>();
                            Logger = new LoggerWork(loggerContext);
                        }

                        Logger.LogInformation("Log test.");
                        Console.WriteLine("The data is written to the log.");
                        //LoggerWork lw = (LoggerWork)logger;
                        //var logItems = lw.GetLogger();
                        break;

                        //default:
                        //    Console.WriteLine("Unknown Command " + command);
                        //    break;
                }

                if (Regex.IsMatch(command, @"^lg\s+\d+$", RegexOptions.IgnoreCase))
                {
                    if (Logger == null)
                    {
                        MongoDbLoggerConnectionConfig logConfig = configuration.GetSection("MongoDbLoggerConnection").Get<MongoDbLoggerConnectionConfig>();
                        var loggerContext = new LoggerContext(logConfig.Connection, logConfig.DbName);

                        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                        loggerFactory.AddContext(loggerContext);
                        //  var logger = loggerFactory.CreateLogger("NewLogger");

                        var loggerService = serviceProvider.GetService<ILogger>();
                        Logger = new LoggerWork(loggerContext);
                    }

                    var logs = (await Logger.GetLoggerAsync()).Take(2);
                    string json = JsonSerializer.Serialize(logs);
                    Console.WriteLine(json);
                }
            }


            //
            //var logJson = JsonSerializer.Serialize(logItems);
            //Console.WriteLine($"Log: {logJson}");

        }
    }
}

