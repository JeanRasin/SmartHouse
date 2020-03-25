using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;

namespace SmartHouseAPI.Helpers
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ILoggerContext _context;

        public LoggerProvider(ILoggerContext context)
        {
            _context = context;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var loggerRepository = new LoggerRepository<LoggerModel>(_context, categoryName);
            return new LoggerWork(loggerRepository);
        }

        public void Dispose()
        {
        }
    }
}