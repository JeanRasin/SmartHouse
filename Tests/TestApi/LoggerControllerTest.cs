using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.ApiException;
using SmartHouseAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest
{
    [CollectionDefinition("Logger controller")]
    public class LoggerControllerTest
    {
        private static readonly IEnumerable<LoggerModel> _loggerList = GetTestData();

        private readonly Mock<ILoggerWork> _mockLogger;
        private readonly LoggerController _loggerController;

        public LoggerControllerTest()
        {
            _mockLogger = new Mock<ILoggerWork>();
            _loggerController = new LoggerController(_mockLogger.Object);
        }

        static IEnumerable<LoggerModel> GetTestData(int n = 10)
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            SmartHouse.Domain.Core.EventId eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
                  .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                  .RuleFor(o => o.Name, f => f.Random.String2(10))
                  .Generate();

            IEnumerable<LoggerModel> result = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.CategoryName, f => f.Random.Words(1))
                .RuleFor(o => o.EventId, f => eventIdFaker)
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words(20))
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate(n);

            return result;
        }

        #region GetLoggerAsync
        /// <summary>
        /// Get logger data.
        /// </summary>
        [Fact]
        public void GetLoggerAsync_Success_LoggerModelItems()
        {
            // Arrange
            _mockLogger.Setup(m => m.GetLoggerAsync()).Returns(Task.FromResult(_loggerList));

            // Act
            var result = _loggerController.GetLoggerAsync().Result as OkObjectResult;

            // Assert
            Assert.IsAssignableFrom<IEnumerable<LoggerModel>>(result.Value);
            Assert.Equal(result.Value, _loggerList);
        }

        /// <summary>
        /// Not data logger.
        /// </summary>
        [Fact]
        public void GetLoggerAsync_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            _mockLogger.Setup(m => m.GetLoggerAsync()).Returns(Task.FromResult<IEnumerable<LoggerModel>>(null));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _loggerController.GetLoggerAsync());
        }

        /// <summary>
        /// The exception is that the service is not responding.
        /// </summary>
        [Fact]
        public void GetLoggerAsync_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockLogger.Setup(m => m.GetLoggerAsync()).Throws<Exception>();

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _loggerController.GetLoggerAsync());
        }
        #endregion
    }
}
