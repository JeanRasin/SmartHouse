using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace SmartHouse.Business.Data
{
    public class LoggerWork : ILoggerWork, ILogger
    {
        private readonly LoggerRepository<LoggerModel> repository;

        public LoggerWork(ILoggerContext context, string categoryName)
        {
            repository = new LoggerRepository<LoggerModel>(context, categoryName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public async Task<IEnumerable<LoggerModel>> GetLoggerAsync()
        {
            var result = await repository.QueryAsync();
            return result;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var msg = formatter(state, exception) + Environment.NewLine;
                repository.Create(new LoggerModel(logLevel, new Domain.Core.EventId(), msg));
            }
        }
    }
}
