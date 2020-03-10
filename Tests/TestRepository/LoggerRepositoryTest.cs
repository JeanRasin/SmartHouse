using Bogus;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Servers;
using Moq;
using RepositoryTest.Helpers;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EventId = SmartHouse.Domain.Core.EventId;

namespace RepositoryTest
{
    [CollectionDefinition("Logger repository")]
    public class LoggerRepositoryTest
    {
        private readonly Faker<EventId> eventIdFaker;
        private readonly Faker<LoggerModel> loggerModelFaker;

        private readonly Mock<IMongoCollection<LoggerModel>> collection;
        private readonly Mock<ILoggerContext> context;
        private readonly LoggerRepository<LoggerModel> repository;

        public LoggerRepositoryTest()
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            eventIdFaker = new Faker<EventId>()
                .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                .RuleFor(o => o.Name, f => f.Random.String2(10));

            loggerModelFaker = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                .RuleFor(o => o.CategoryName, f => f.Random.Words(2))
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words(20))
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

            collection = new Mock<IMongoCollection<LoggerModel>>();

            context = new Mock<ILoggerContext>();
            context.Setup(l => l.DbSet<LoggerModel>()).Returns(collection.Object);

            repository = new LoggerRepository<LoggerModel>(context.Object, "test category");
        }

        #region Create
        /// <summary>
        /// Create logger.
        /// </summary>
        [Fact]
        public void Create_Success()
        {
            // Arrange
            LoggerModel testData = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.CategoryName, f => f.Random.Words(2))
                .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words())
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate();

            collection.Setup(m => m.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));

            // Act
            repository.Create(testData);

            // Assert
            collection.Verify(v => v.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once, "InsertOne was not called.");
        }

        /// <summary>
        /// Throw exception MongoWriteException.
        /// </summary>
        [Fact]
        public void Create_IdNotFound_MongoWriteException()
        {
            // Arrange
            static MongoWriteException MongoWriteExceptionObj()
            {
                var connectionId = new ConnectionId(new ServerId(new ClusterId(1), new DnsEndPoint("localhost", 27017)), 2);
                var innerException = new Exception("inner");

                ConstructorInfo ctor = typeof(WriteError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                WriteError writeError = (WriteError)ctor.Invoke(new object[] { ServerErrorCategory.Uncategorized, 1, "writeError", new BsonDocument("details", "writeError") });

                ctor = typeof(WriteConcernError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
                WriteConcernError writeConcernError = (WriteConcernError)ctor.Invoke(new object[] { 1, "writeConcernError", "", new BsonDocument("details", "writeConcernError") });

                return new MongoWriteException(connectionId, writeError, writeConcernError, innerException);
            };

            var testData = new Faker<LoggerModel>()
                // Of validation model.
                // .StrictMode(true)
                // Id null.
                // .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.CategoryName, f => f.Random.Words(2))
                .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words())
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate();

            collection.Setup(m => m.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Throws(MongoWriteExceptionObj());

            // Act & Assert
            Assert.Throws<MongoWriteException>(() => repository.Create(testData));
        }
        #endregion

        #region Query
        /// <summary>
        /// Get all the loggers.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Query_All_Data()
        {
            // Arrange
            List<LoggerModel> items = loggerModelFaker.Generate(2);

            collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            // Act
            List<LoggerModel> result = (await repository.QueryAsync()).ToList();

            // Assert
            collection.Verify(v => v.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()), "FindAsync was not called.");
            Assert.NotNull(result);
            Assert.Equal(items, result);
        }
        #endregion

        #region QueryFilter
        /// <summary>
        /// Get filtered data.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task QueryFilter_FilterId_Data()
        {
            // Arrange
            List<LoggerModel> items = loggerModelFaker.Generate(1);
            LoggerModel item = items.Single();

            collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            // Act
            List<LoggerModel> result = (await repository.QueryAsync(s => s.Id == item.Id)).ToList();

            // Assert
            collection.Verify(v => v.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()), "FindAsync was not called.");
            Assert.NotNull(result);
            Assert.Equal(items, result);
        }
        #endregion
    }
}
