using Microsoft.AspNetCore.Builder;
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

namespace SmartHouseAPI
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
            services.AddControllers();
            // services.AddLogging(cfg => cfg.AddConsole());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo { Title = "Api Docs", Version = "V1" });
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
            services.AddDbContext<GoalContext>(options =>
            {
                options.UseNpgsql(connection);

                //ILoggerFactory loggerFactory = Helpers.LoggerExtensions.AddContext(loggerContext);

                //options.UseLoggerFactory(loggerFactory);
            });
            //services.AddControllersWithViews();

            // services.AddTransient<IGoalWork<GoalWork>, GoalWork>();

            services.AddTransient<IGoalWork<GoalModel>, GoalWork>();
            services.AddTransient<IWeatherWork, WeatherWork>();
            services.AddTransient<ILoggerWork, LoggerWork>();

            services.AddSingleton<ILogger, LoggerWork>();

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
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();

            // обработка ошибок HTTP
            app.UseStatusCodePages("text/plain", "Error. Status code : {0}");
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();

           // app.UseMiddleware<RequestResponseLoggingMiddleware>();

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
