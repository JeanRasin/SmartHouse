using Bogus;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using RepositoryTest.Helpers;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using EventId = SmartHouse.Domain.Core.EventId;

namespace RepositoryTest
{
    public class LoggerRepositoryTest
    {
        private readonly Faker<EventId> eventIdFaker;
        private readonly Faker<LoggerModel> loggerModelFaker;

        public LoggerRepositoryTest()
        {
            Randomizer.Seed = new Random(1338);

            eventIdFaker = new Faker<EventId>()
                .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                .RuleFor(o => o.Name, f => f.Random.String2(10));

            loggerModelFaker = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words());
        }

        [Fact]
        public void Repository_InsertOne_void()
        {
            //var data = new LoggerModel
            //{
            //    Id = "1",
            //    EventId = new EventId(1),
            //    LogLevel = LogLevel.Information,
            //    Message = "test 1"
            //};

            var data = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words());

            var collection = new Mock<IMongoCollection<LoggerModel>>();
            collection.Setup(m => m.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));

            var context = new Mock<LoggerContext>();
            context.Setup(l => l.DbSet<LoggerModel>()).Returns(collection.Object);

            var repository = new LoggerRepository<LoggerModel>(context.Object);
            repository.Create(data);

            Assert.True(true);
        }

        [Fact]
        public void Repository_Query_data()
        {
            //var items = new List<LoggerModel> {
            //new LoggerModel
            //{
            //     Id = "1",
            //    EventId = new EventId(1),
            //    LogLevel = LogLevel.Information,
            //       Message = "test 1"
            //},
            //new LoggerModel
            //{
            //    Id = "2",
            //    EventId = new EventId(1),
            //    LogLevel = LogLevel.Information,
            //    Message = "test 2"
            //}};

            var items = loggerModelFaker.Generate(2);

            var collection = new Mock<IMongoCollection<LoggerModel>>();
            collection
                 .Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            var context = new Mock<LoggerContext>();
            context.Setup(l => l.DbSet<LoggerModel>()).Returns(collection.Object);

            var repository = new LoggerRepository<LoggerModel>(context.Object);
            List<LoggerModel> result = repository.QueryAsync().Result.ToList();

            Assert.NotNull(result);
            Assert.Equal(items, result);
        }

        [Fact]
        public void Repository_QueryFilter_data()
        {
            //var items = new List<LoggerModel> {
            //new LoggerModel
            //{
            //   Id = "1",
            //    EventId = new EventId(1),
            //    LogLevel = LogLevel.Information,
            //    Message = "test 1"
            //},
            //new LoggerModel
            //{
            //    Id = "2",
            //    EventId = new EventId(1),
            //    LogLevel = LogLevel.Information,
            //    Message = "test 2"
            //}};

            var items = loggerModelFaker.Generate(2);

            var collection = new Mock<IMongoCollection<LoggerModel>>();
            collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            var context = new Mock<LoggerContext>();
            context.Setup(l => l.DbSet<LoggerModel>()).Returns(collection.Object);

            var repository = new LoggerRepository<LoggerModel>(context.Object);

            var query = repository.QueryAsync(s => s.Id == "1");

            Assert.NotNull(query);
            Assert.Equal(items, query.Result);
        }
    }
}