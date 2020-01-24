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

namespace SmartHouseAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            // �������� ������ ����������� �� ����� ������������
            string connection = Configuration.GetConnectionString("DefaultConnection");
            // ��������� �������� MobileContext � �������� ������� � ����������
            services.AddDbContext<GoalContext>(options =>
                options.UseNpgsql(connection));
            //services.AddControllersWithViews();

            services.AddTransient<IGoalWork, GoalWork>();

            IDictionary<string, string> parm = Configuration.GetSection("OpenWeatherMapService").Get<OpenWeatherMapServiceConfig>().ToDictionary<string>();
            services.AddTransient<IWeatherService>(x => new OpenWeatherMapService(x.GetRequiredService<ILogger<OpenWeatherMapService>>(), parm)); // OpenWeatherMap service.
            //services.AddTransient<IWeatherService, GisMeteoService>(); // GisMeteo service.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("Staging_2"))
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            app.UseStaticFiles();

            // ��������� ������ HTTP
            app.UseStatusCodePages("text/plain", "Error. Status code : {0}");
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

           // app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();

            MongoDbLoggerConnectionConfig logConfig = Configuration.GetSection("MongoDbLoggerConnection").Get<MongoDbLoggerConnectionConfig>();
            var loggerContext = new LoggerContext(logConfig.Connection, logConfig.DbName);

            loggerFactory.AddContext(loggerContext);


            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // ����������� ���������
                // endpoints.MapControllerRoute(
                //  name: "default",
                //   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
