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
using CommandLine;
using SmartHouse.Domain.Interfaces;

namespace ConsoleAPI
{
    //[Verb("add", HelpText = "Add file contents to the index.")]
    //class AddOptions
    //{
    //    //normal options here
    //}
    //[Verb("commit", HelpText = "Record changes to the repository.")]
    //class CommitOptions
    //{
    //    //commit options here
    //}
    //[Verb("clone", HelpText = "Clone a repository into a new directory.")]
    //class CloneOptions
    //{
    //    //clone options here
    //}

    class Program
    {
        //public class Options
        //{
        //    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        //    public bool Verbose { get; set; }
        //}

        static void Main(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var weatherCachingTime = configuration["WeatherCachingTime"];

            IDictionary<string, string> parm = configuration.GetSection("OpenWeatherMapService").Get<OpenWeatherMapServiceConfig>().ToDictionary<string>();

            MongoDbLoggerConnectionConfig logConfig = configuration.GetSection("MongoDbLoggerConnection").Get<MongoDbLoggerConnectionConfig>();
            var loggerContext = new LoggerContext(logConfig.Connection, logConfig.DbName);

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

            string connectStr = configuration.GetConnectionString("DefaultConnection");
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

            var weather = weatherWork.GetWeatherAsync().Result;
            var weatherJson = JsonSerializer.Serialize(weather);

            Console.WriteLine($"Weather: {weatherJson}");
            /*
              Parser.Default.ParseArguments<Options>(args)
                     .WithParsed<Options>(o =>
                     {
                         if (o.Verbose)
                         {
                             Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                             Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                         }
                         else
                         {
                             Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                             Console.WriteLine("Quick Start Example!");
                         }
                     });  */

            //Console.WriteLine("Exit");
        }
    }
}
