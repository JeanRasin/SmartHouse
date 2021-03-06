using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces.Contexts;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Helpers;
using SmartHouse.Services.Interfaces;
using SmartHouseAPI.Extensions;
using SmartHouseAPI.Helpers;
using System;

namespace SmartHouseAPI
{
    public class Startup
    {
        private const string allowSpecificOrigins = "_allowSpecificOrigins";

        private ILoggerContext _loggerContext = null;
        private readonly IWebHostEnvironment _currentEnvironment;

        private bool IsLogger { get { return Convert.ToBoolean(Configuration["Logger"]); } }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _currentEnvironment = env;
        }

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
                 });

            // Register the Swagger generator, defining 1 or more Swagger documents.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(allowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(Configuration["FrontUrlsCors"].Split(";"))
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddControllersWithViews(options =>
            {
                string weatherCachingTime = Configuration["WeatherCachingTime"];

                options.CacheProfiles.Add("WeatherCaching",
                    new CacheProfile()
                    {
                        Duration = (int)TimeSpan.Parse(weatherCachingTime).TotalSeconds
                    });
            });

            if (_currentEnvironment.IsDevelopment())
            {
                int randomizerSeed = Convert.ToInt32(Configuration["RandomizerSeed"]);

                // Random const.
                Randomizer.Seed = new Random(randomizerSeed);
            }

            services.RegisterServices(Configuration, _currentEnvironment);

            if (IsLogger)
            {
                MongoSettings logConfig = Configuration.GetSection("MongoDbLoggerConnection").Get<MongoSettings>();
                _loggerContext = new Contexts.LoggerContext(logConfig);

                if (_currentEnvironment.IsDevelopment())
                {
                    _loggerContext.EnsureCreated();
                }

                services.AddScoped<ILoggerWork>(x => new LoggerWork(new LoggerRepository<Logger>(_loggerContext, "General category")));
            }
            else
            {
                services.AddLogging(cfg => cfg.AddConsole());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            if (IsLogger)
            {
                loggerFactory.AddContext(_loggerContext);
            }

            app.UseStaticFiles();

            // Creates Swagger JSON.
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

            app.UseRouting();

            // CORS policy.
            app.UseCors(allowSpecificOrigins);

            // Application registration.s
            app.RegisterApplicationBuilders();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}