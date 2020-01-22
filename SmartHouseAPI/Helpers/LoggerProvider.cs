using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Infrastructure.Data;

namespace SmartHouseAPI.Helpers
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly LoggerContext context;
        public LoggerProvider(LoggerContext context)
        {
            this.context = context;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new LoggerWork(context);
        }

        public void Dispose()
        {
        }
    }
}
