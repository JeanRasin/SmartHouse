using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartHouse.Business.Data
{
    public interface ILoggerWork
    {
        List<LoggerModel> GetLogger();
        bool WriteLog(string message);
    }

    //public class LoggerWork : ILoggerWork
    public class LoggerWork : ILogger
    {
        private readonly LoggerRepository<LoggerModel> repository;
        //private static object _lock = new object();

        public LoggerWork(LoggerContext context)
        {
            repository = new LoggerRepository<LoggerModel>(context);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public List<LoggerModel> GetLogger()
        {
            var result = repository.Query().ToList();
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
                // lock (_lock)
                // {
                //File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                var msg = formatter(state, exception) + Environment.NewLine;
                repository.Create(new LoggerModel(msg));
                // }
            }
        }

        //public bool WriteLog(string message)
        //{
        //    var result = repository.Create(new LoggerModel { Id = "5", Message = message });
        //    return result; //todo:!!!
        //}
    }
}
