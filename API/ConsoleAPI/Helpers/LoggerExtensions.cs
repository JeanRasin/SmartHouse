using Microsoft.Extensions.Logging;
using SmartHouse.Infrastructure.Data;

namespace ConsoleAPI.Helpers
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddContext(this ILoggerFactory factory, LoggerContext context)
        {
            factory.AddProvider(new LoggerProvider(context));
            return factory;
        }
    }
}
