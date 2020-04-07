using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Interfaces.Contexts;

namespace SmartHouseAPI.Helpers
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddContext(this ILoggerFactory factory, ILoggerContext context)
        {
            factory.AddProvider(new LoggerProvider(context));
            return factory;
        }
    }
}