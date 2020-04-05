using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Services.Interfaces;
using SmartHouseAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest
{
    [Collection("Logger controller")]
    public class LoggerControllerTest
    {
        private static readonly IEnumerable<Logger> _loggerList = GetTestData();

        private readonly Mock<ILoggerWork> _mockLogger;
        private readonly LoggerController _loggerController;

        public LoggerControllerTest()
        {
            _mockLogger = new Mock<ILoggerWork>();
            _loggerController = new LoggerController(_mockLogger.Object);
        }

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static IEnumerable<Logger> GetTestData(int n = 10)
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            SmartHouse.Domain.Core.EventId eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
                  .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                  .RuleFor(o => o.Name, f => f.Random.String2(10))
                  .Generate();

            IEnumerable<Logger> result = new Faker<Logger>()
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
        [Trait("Get Logger Data", "Success Status 200")]
        public void GetLoggerAsync_Success_LoggerModelItems()
        {
            // Arrange
            _mockLogger.Setup(m => m.GetLoggerAsync()).Returns(Task.FromResult(_loggerList));

            // Act
            var result = _loggerController.GetLoggerAsync().Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Logger>>(result.Value);
            Assert.Equal(result.Value, _loggerList);
            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// The exception is that the service is not responding.
        /// </summary>
        [Fact]
        [Trait("Get Logger Data", "Exception Status 500")]
        public void GetLoggerAsync_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockLogger.Setup(m => m.GetLoggerAsync()).Throws<Exception>();

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _loggerController.GetLoggerAsync());
        }

        #endregion GetLoggerAsync
    }
}