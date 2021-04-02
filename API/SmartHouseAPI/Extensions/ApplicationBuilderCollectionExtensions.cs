using Microsoft.AspNetCore.Builder;
using SmartHouseAPI.Middleware;

namespace SmartHouseAPI.Extensions
{
    public static class ApplicationBuilderCollectionExtensions
    {
        /// <summary>
        /// Регистрация приложений.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterApplicationBuilders(this IApplicationBuilder app)
        {
            // Exception logger write.
            app.UseMiddleware<ExceptionMiddleware>();

            // Logger write.
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            return app;
        }
    }
}
