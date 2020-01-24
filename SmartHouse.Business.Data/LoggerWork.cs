using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHouse.Business.Data
{
    public class LoggerWork : ILogger
    {
        private readonly LoggerRepository<LoggerModel> repository;
        public LoggerWork(LoggerContext context)
        {
            repository = new LoggerRepository<LoggerModel>(context);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public Task<IEnumerable<LoggerModel>> GetLoggerAsync()
        {
            var result = repository.QueryAsync();
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
                repository.Create(new LoggerModel(msg));
            }
        }

    }
}
