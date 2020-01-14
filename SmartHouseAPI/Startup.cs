using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Interfaces.Weather;
using SmartHouse.Infrastructure.Data.Weather;

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
            services.AddLogging(cfg => cfg.AddConsole());

            var parm = new Dictionary<string, string>
            {
                { "city", "Perm,ru" },
                { "api", "f4c946ac33b35d68233bbcf83619eb58" }
            };

            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DefaultConnection");
            // добавляем контекст MobileContext в качестве сервиса в приложение
            services.AddDbContext<ActContext>(options =>
                options.UseNpgsql(connection));
            //services.AddControllersWithViews();

            services.AddTransient<IGoalWork, GoalWork>();
            services.AddTransient<IWeatherService>(x => new OpenWeatherService(x.GetRequiredService<ILogger<OpenWeatherService>>(), parm));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

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
