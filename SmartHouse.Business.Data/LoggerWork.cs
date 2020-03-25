using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace SmartHouse.Business.Data
{
    public class LoggerWork : ILoggerWork, ILogger
    {
        private readonly ILoggerRepository<LoggerModel> _repository;

        public LoggerWork(ILoggerRepository<LoggerModel> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Stub.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Get logger data.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoggerModel>> GetLoggerAsync()
        {
            IEnumerable<LoggerModel> result = await _repository.QueryAsync();
            return result;
        }

        /// <summary>
        /// Enabled stub.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Write log.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                string msg = formatter(state, exception) + Environment.NewLine;
                _repository.Create(new LoggerModel(logLevel, new Domain.Core.EventId(eventId), msg));
            }
        }
    }
}