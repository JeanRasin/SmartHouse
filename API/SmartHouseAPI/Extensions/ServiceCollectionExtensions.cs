using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Domain.Interfaces.Contexts;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Helpers;
using SmartHouse.Service.Weather.OpenWeatherMap;
using SmartHouse.Services.Interfaces;
using SmartHouseAPI.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SmartHouseAPI.Extensions
{
    /// <summary>
    /// Расширения IServiceColelction.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрация сервисов приложения.
        /// </summary>
        /// <param name="services">Объект ServiceCollection.</param>
        public static IServiceCollection RegisterServices(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment currentEnvironment,
            ILoggerContext loggerContext, bool isLogger)
        {
            #region PostgreSql

            services.AddTransient(_ =>
            {
                string connection = configuration.GetConnectionString("DefaultConnection");

                var options = new DbContextOptionsBuilder<GoalContext>();
                options.UseNpgsql(connection);

                if (currentEnvironment.IsDevelopment())
                {
                    var goalContext = new Contexts.GoalContext(options.Options);
                    goalContext.Database.EnsureCreated();
                    return goalContext;
                }
                else
                {
                    var goalContext = new GoalContext(options.Options);
                    goalContext.Database.EnsureCreated();
                    return goalContext;
                }
            });

            #endregion

            #region Redis

            services.AddTransient(_ =>
            {
                var redisConnectString = configuration
                .GetSection("RedisConnection")
                .Get<RedisSettings>();

                ConfigurationOptions options = ConfigurationOptions
                .Parse(redisConnectString.Connection);
                options.Password = redisConnectString.Password;

                ConnectionMultiplexer redis = ConnectionMultiplexer
                .Connect(options);

                IDatabase db = redis.GetDatabase(redisConnectString.DatabaseId);

                return db;
            });

            #endregion

            #region Logger
            /*
            if (isLogger)
            {
                MongoSettings logConfig = configuration
                    .GetSection("MongoDbLoggerConnection")
                    .Get<MongoSettings>();

                loggerContext = new Contexts.LoggerContext(logConfig);

                if (currentEnvironment.IsDevelopment())
                {
                    loggerContext.EnsureCreated();
                }

                services.AddScoped<ILoggerWork>(x =>
                new LoggerWork(
                    new LoggerRepository<Logger>(loggerContext, "General category")));
            }
            else
            {
                services.AddLogging(cfg => cfg.AddConsole());
            }
            */
            #endregion

            #region Weather service
            
            IDictionary<string, string> parm = configuration
                .GetSection("OpenWeatherMapService")
                .Get<OpenWeatherMapServiceConfig>()
                .ToDictionary<string>();

            // OpenWeatherMap service.
            services.AddScoped<IWeatherService>(_ =>
            new OpenWeatherMapService(parm,
            new HttpClient(),
            logger: _.GetRequiredService<ILogger<OpenWeatherMapService>>()));

            // GisMeteo service.
            //services.AddTransient<IWeatherService, GisMeteoService>();
 
            #endregion
          
            services.AddScoped<IGoalWork<Goal>, GoalWork>();

            var redisWeatherCachingTime = configuration
                .GetSection("RedisWeatherCachingTime")
                .Get<TimeSpan>();

            services.AddScoped<IWeatherWork, WeatherWork>(_ =>
            new WeatherWork(_.GetRequiredService<IWeatherService>(),
            _.GetRequiredService<IDatabase>(), redisWeatherCachingTime, 30));

            #region Automapper

            //var autoMapperConfig = new MapperConfiguration(cfg =>
            //    {
            //        cfg.AddMaps(typeof(MappingAssemblyAnchor).Assembly);
            //    });

            //var mapper = autoMapperConfig.CreateMapper();

            //services.AddSingleton(mapper);
            //services.AddSingleton<IAutoMapperConverter, AutoMapperConverter>();

            #endregion


            #region AutoDependencies

            //var sqlAssembly = typeof(ISqlQuery).Assembly;

            //services.RegisterAssemblyTypes(sqlAssembly)
            //    .Where(t => t.Name.EndsWith("SqlQuery")
            //            && t.GetInterfaces().Any(ti =>
            //                ti.Name == typeof(ISqlQuery).Name))
            //    .AsScoped()
            //    .Bind();

            //services.RegisterAssemblyTypesByName(sqlAssembly,
            //    t => t.EndsWith("Repository"))
            //    .AsScoped()
            //    .AsImplementedInterfaces()
            //    .Bind();

            //services.RegisterAssemblyTypesByName(sqlAssembly,
            //    t => t.EndsWith("RepositorySqlQueries"))
            //    .AsScoped()
            //    .PropertiesAutowired()
            //    .Bind();

            #endregion

            return services;
        }
    }
}
