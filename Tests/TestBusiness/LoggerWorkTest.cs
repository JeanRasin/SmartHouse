using Bogus;
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
    [CollectionDefinition("Logger work")]
    public class LoggerWorkTest
    {
        private readonly Mock<ILoggerRepository<LoggerModel>> mockLoggerRepository;
        private readonly LoggerWork loggerWork;

        public LoggerWorkTest()
        {
            mockLoggerRepository = new Mock<ILoggerRepository<LoggerModel>>();
            loggerWork = new LoggerWork(mockLoggerRepository.Object);
        }

        #region GetLoggerAsync
        [Fact]
        public async void GetLoggerAsync_LoggerModelItems()
        {
            // Arrange
            // Random constant.
            Randomizer.Seed = new Random(1338);

            var eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
                  .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                  .RuleFor(o => o.Name, f => f.Random.String2(10))
                  .Generate();

            var loggerList = new Faker<LoggerModel>()
                         .StrictMode(true)
                         .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                         .RuleFor(o => o.CategoryName, f => f.Random.Words(1))
                         .RuleFor(o => o.EventId, f => eventIdFaker)
                         .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                         .RuleFor(o => o.Message, f => f.Random.Words(20))
                         .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                         .Generate(10);

            mockLoggerRepository.Setup(s => s.QueryAsync()).ReturnsAsync(loggerList);

            // Act
            IEnumerable<LoggerModel> result = await loggerWork.GetLoggerAsync();

            // Assert
            Assert.Equal(result, loggerList);
        }
        #endregion

        #region Log
        /// <summary>
        /// Log write.
        /// </summary>
        [Fact]
        public void Log_LogWrite_Success()
        {
            // Arrange
            static string formatterFunc(string state, Exception exception)
            {
                return exception?.Message ?? state.ToString();
            };

            mockLoggerRepository
                .Setup(s => s.Create(It.IsAny<LoggerModel>()))
                .Verifiable();

            // Act
            loggerWork.Log(
              LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(1),
                "text",
                new Exception("test exception"),
                formatterFunc
                );

            // Assert
            mockLoggerRepository.Verify(v => v.Create(It.IsAny<LoggerModel>()), Times.Once());
        }

        /// <summary>
        /// Log write formatter is null.
        /// </summary>
        [Fact]
        public void Log_FormatterIsNull_LogWrite()
        {
            // Arrange
            mockLoggerRepository
                .Setup(s => s.Create(It.IsAny<LoggerModel>()))
                .Verifiable();

            // Act
            loggerWork.Log(
              LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(1),
                "text",
                new Exception("test exception"),
                null // Formatter is null.
                );

            // Assert
            mockLoggerRepository.Verify(v => v.Create(It.IsAny<LoggerModel>()), Times.Never);
        }
        #endregion

        #region BeginScope
        /// <summary>
        /// Method BeginScope is null.
        /// </summary>
        [Fact]
        public void BeginScope_null()
        {
            // Act
            IDisposable result = loggerWork.BeginScope(new object());

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region IsEnabled
        /// <summary>
        /// Method IsEnabled is true.
        /// </summary>
        [Fact]
        public void IsEnabled_true()
        {
            // Act
            bool result = loggerWork.IsEnabled(new LogLevel());

            // Assert
            Assert.True(result);
        }
        #endregion
    }
}
