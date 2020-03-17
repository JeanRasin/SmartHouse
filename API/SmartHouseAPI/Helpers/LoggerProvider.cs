using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;

namespace SmartHouseAPI.Helpers
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ILoggerContext context;
        public LoggerProvider(ILoggerContext context)
        {
            this.context = context;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var loggerRepository = new LoggerRepository<LoggerModel>(context, categoryName);
            return new LoggerWork(loggerRepository);
        }

        public void Dispose()
        {
        }
    }
}
