﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Service.Weather.Gismeteo;
using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;
using SmartHouseAPI.Helpers;
using SmartHouse.Service.Weather.OpenWeatherMap;
using SmartHouse.Domain.Interfaces;
using Bogus;
using SmartHouse.Domain.Core;
using SmartHouseAPI.Middleware;
using static SmartHouseAPI.Middleware.RequestResponseLoggingMiddleware;
using System.Diagnostics;

namespace ApiIntegrationTest
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                 .ConfigureApiBehaviorOptions(options =>
                 {
                     options.SuppressConsumesConstraintForFormFileParameters = true;
                     options.SuppressInferBindingSourcesForParameters = true;
                     options.SuppressModelStateInvalidFilter = true;
                     options.SuppressMapClientErrors = true;
                     //  options.ClientErrorMapping[404].Link ="https://httpstatuses.com/404";
                 });
            // services.AddLogging(cfg => cfg.AddConsole());

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddControllersWithViews(options =>
            {
                var weatherCachingTime = Configuration["WeatherCachingTime"];

                options.CacheProfiles.Add("WeatherCaching",
                    new CacheProfile()
                    {
                        Duration = (int)TimeSpan.Parse(weatherCachingTime).TotalSeconds
                    });
            });

            // string envName = CurrentEnvironment.IsDevelopment;

            if (CurrentEnvironment.IsDevelopment())
            {
                int randomizerSeed = Convert.ToInt32(Configuration["RandomizerSeed"]);
                // Random const;
                Randomizer.Seed = new Random(randomizerSeed);
            }

            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DefaultConnection");

            MongoDbLoggerConnectionConfig logConfig = Configuration.GetSection("MongoDbLoggerConnection").Get<MongoDbLoggerConnectionConfig>();
            ILoggerContext loggerContext = new LoggerContext(logConfig.Connection, logConfig.DbName);

            // добавляем контекст MobileContext в качестве сервиса в приложение
            //services.AddDbContext<GoalContext>(options =>
            //{
            //    options.UseNpgsql(connection);

            //    //ILoggerFactory loggerFactory = Helpers.LoggerExtensions.AddContext(loggerContext);

            //    //options.UseLoggerFactory(loggerFactory);
            //});

            services.AddTransient<SmartHouse.Infrastructure.Data.GoalContext>(x => {

                //var options = new DbContextOptions<SmartHouse.Infrastructure.Data.GoalContext>();
                var options = new DbContextOptionsBuilder<SmartHouse.Infrastructure.Data.GoalContext>();
                options.UseNpgsql(connection);

                return new SmartHouseAPI.GoalContext(options.Options, CurrentEnvironment);
            });

            //services.AddControllersWithViews();

            // services.AddTransient<IGoalWork<GoalWork>, GoalWork>();

            services.AddTransient<IGoalWork<GoalModel>, GoalWork>();
            services.AddTransient<IWeatherWork, WeatherWork>();
            services.AddTransient<ILoggerWork>(x => new LoggerWork(x.GetRequiredService<ILoggerContext>(), "General category"));

            IDictionary<string, string> parm = Configuration.GetSection("OpenWeatherMapService").Get<OpenWeatherMapServiceConfig>().ToDictionary<string>();

            // OpenWeatherMap service.
            services.AddTransient<IWeatherService>(x => new OpenWeatherMapService(parm, logger: x.GetRequiredService<ILogger<OpenWeatherMapService>>()));
            // GisMeteo service.
            //services.AddTransient<IWeatherService, GisMeteoService>(); 

            services.AddSingleton(loggerContext);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, ILoggerContext loggerContext)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            loggerFactory.AddContext(loggerContext);
            app.UseStaticFiles();

            /*
            Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
            {
                Debug.Print(requestProfilerModel.Request);
                Debug.Print(Environment.NewLine);
                Debug.Print(requestProfilerModel.Response);
            };

            app.UseMiddleware<RequestResponseLoggingMiddleware>(requestResponseHandler);
            */

           // app.UseMiddleware<RequestResponseLoggingMiddleware>();

           // app.UseMiddleware<ExceptionMiddleware>();


            // обработка ошибок HTTP
            //app.UseStatusCodePages("text/plain", "Error. Status code : {0}");

            // Creates Swagger JSON
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/docs/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/docs/v1/swagger.json", "AnnexUI API V1");
                c.RoutePrefix = "api/docs";
            });

            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();

            //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // определение маршрутов
                // endpoints.MapControllerRoute(
                //  name: "default",
                //   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
