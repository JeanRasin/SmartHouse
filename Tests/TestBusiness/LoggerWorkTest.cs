﻿using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace BusinessTest
{
    [Collection("Logger work")]
    public class LoggerWorkTest
    {
        private readonly Mock<ILoggerRepository<Logger>> _mockLoggerRepository;
        private readonly LoggerWork _loggerWork;

        public LoggerWorkTest()
        {
            _mockLoggerRepository = new Mock<ILoggerRepository<Logger>>();
            _loggerWork = new LoggerWork(_mockLoggerRepository.Object);
        }

        #region GetLoggerAsync

        [Fact]
        [Trait("GetLoggerAsync", "Success")]
        public async void GetLoggerAsync_LoggerModelItems()
        {
            // Arrange
            // Random constant.
            Randomizer.Seed = new Random(1338);

            var eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
                  .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                  .RuleFor(o => o.Name, f => f.Random.String2(10))
                  .Generate();

            var loggerList = new Faker<Logger>()
                         .StrictMode(true)
                         .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                         .RuleFor(o => o.CategoryName, f => f.Random.Words(1))
                         .RuleFor(o => o.EventId, f => eventIdFaker)
                         .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                         .RuleFor(o => o.Message, f => f.Random.Words(20))
                         .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                         .Generate(10);

            _mockLoggerRepository.Setup(s => s.QueryAsync()).ReturnsAsync(loggerList);

            // Act
            IEnumerable<Logger> result = await _loggerWork.GetLoggerAsync();

            // Assert
            Assert.Equal(result, loggerList);
        }

        #endregion GetLoggerAsync

        #region Log

        /// <summary>
        /// Log write.
        /// </summary>
        [Fact]
        [Trait("Log Write", "Success")]
        public void Log_LogWrite_Success()
        {
            // Arrange
            static string formatterFunc(string state, Exception exception)
            {
                return exception?.Message ?? state.ToString();
            };

            _mockLoggerRepository
                .Setup(s => s.Create(It.IsAny<Logger>()))
                .Verifiable();

            // Act
            _loggerWork.Log(
              LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(1),
                "text",
                new Exception("test exception"),
                formatterFunc
                );

            // Assert
            _mockLoggerRepository.Verify(v => v.Create(It.IsAny<Logger>()), Times.Once());
        }

        /// <summary>
        /// Log write formatter is null.
        /// </summary>
        [Fact]
        [Trait("Log Write", "Formatter Is Null")]
        public void Log_FormatterIsNull_LogWrite()
        {
            // Arrange
            _mockLoggerRepository
                .Setup(s => s.Create(It.IsAny<Logger>()))
                .Verifiable();

            // Act
            _loggerWork.Log(
              LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(1),
                "text",
                new Exception("test exception"),
                null // Formatter is null.
                );

            // Assert
            _mockLoggerRepository.Verify(v => v.Create(It.IsAny<Logger>()), Times.Never);
        }

        #endregion Log

        #region BeginScope

        /// <summary>
        /// Method BeginScope is null.
        /// </summary>
        [Fact]
        [Trait("BeginScope", "Null")]
        public void BeginScope_null()
        {
            // Act
            IDisposable result = _loggerWork.BeginScope(new object());

            // Assert
            Assert.Null(result);
        }

        #endregion BeginScope

        #region IsEnabled

        /// <summary>
        /// Method IsEnabled is true.
        /// </summary>
        [Fact]
        [Trait("IsEnabled", "True")]
        public void IsEnabled_true()
        {
            // Act
            bool result = _loggerWork.IsEnabled(new LogLevel());

            // Assert
            Assert.True(result);
        }

        #endregion IsEnabled
    }
}