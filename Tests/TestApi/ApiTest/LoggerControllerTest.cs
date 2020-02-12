﻿using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest
{
    public class LoggerControllerTest
    {
        readonly IEnumerable<LoggerModel> loggerList;

        public LoggerControllerTest()
        {
            Randomizer.Seed = new Random(1338);

            var eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
                  .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                  .RuleFor(o => o.Name, f => f.Random.String2(10))
                  .Generate();

            loggerList = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.EventId, f => eventIdFaker)
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words(20))
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate(10);
        }

        [Fact]
        public void GetLoggerAsync_WhenCalled_Returnsresult()
        {
            var mockLogger = new Mock<ILoggerWork>();

            mockLogger.Setup(m => m.GetLoggerAsync()).Returns(() =>
            {
                return Task.FromResult(loggerList);
            });

            var logController = new LoggerController(mockLogger.Object);
            var result = logController.GetLoggerAsync().Result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<LoggerModel>>(result.Value);
            Assert.Equal(result.Value, loggerList);
        }

        [Fact]
        public void GetLoggerAsync_WhenCalled_ReturnsStatus404()
        {
            var mockLogger = new Mock<ILoggerWork>();

            mockLogger.Setup(m => m.GetLoggerAsync()).Returns(() =>
            {
                return Task.FromResult<IEnumerable<LoggerModel>>(null);
            });

            var loggerController = new LoggerController(mockLogger.Object);
            var result = loggerController.GetLoggerAsync().Result as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetLoggerAsync_WhenCalled_ReturnsStatus500()
        {
            var mockLogger = new Mock<ILoggerWork>();

            mockLogger.Setup(m => m.GetLoggerAsync()).Throws<Exception>();

            var loggerController = new LoggerController(mockLogger.Object);
            var result = loggerController.GetLoggerAsync().Result as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
