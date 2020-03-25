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
    [Collection("Logger repository")]
    public class LoggerRepositoryTest
    {
        private readonly Faker<EventId> _eventIdFaker;
        private readonly Faker<LoggerModel> _loggerModelFaker;

        private readonly Mock<IMongoCollection<LoggerModel>> _collection;
        private readonly Mock<ILoggerContext> _context;
        private readonly LoggerRepository<LoggerModel> _repository;

        public LoggerRepositoryTest()
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            _eventIdFaker = new Faker<EventId>()
                .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                .RuleFor(o => o.Name, f => f.Random.String2(10));

            _loggerModelFaker = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.EventId, f => _eventIdFaker.Generate())
                .RuleFor(o => o.CategoryName, f => f.Random.Words(2))
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words(20))
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

            _collection = new Mock<IMongoCollection<LoggerModel>>();

            _context = new Mock<ILoggerContext>();
            _context.Setup(l => l.DbSet<LoggerModel>()).Returns(_collection.Object);

            _repository = new LoggerRepository<LoggerModel>(_context.Object, "test category");
        }

        #region Create

        /// <summary>
        /// Create logger.
        /// </summary>
        [Fact]
        [Trait("Create", "Success")]
        public void Create_Success()
        {
            // Arrange
            LoggerModel testData = new Faker<LoggerModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                .RuleFor(o => o.CategoryName, f => f.Random.Words(2))
                .RuleFor(o => o.EventId, f => _eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words())
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate();

            _collection.Setup(m => m.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()));

            // Act
            _repository.Create(testData);

            // Assert
            _collection.Verify(v => v.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once, "InsertOne was not called.");
        }

        /// <summary>
        /// Throw exception MongoWriteException.
        /// </summary>
        [Fact]
        [Trait("Create", "MongoWriteException")]
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
                .RuleFor(o => o.EventId, f => _eventIdFaker.Generate())
                .RuleFor(o => o.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(o => o.Message, f => f.Random.Words())
                .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .Generate();

            _collection.Setup(m => m.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Throws(MongoWriteExceptionObj());

            // Act & Assert
            Assert.Throws<MongoWriteException>(() => _repository.Create(testData));
        }

        #endregion Create

        #region Query

        /// <summary>
        /// Get all the loggers.
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("Query", "Success")]
        public async Task Query_All_Data()
        {
            // Arrange
            List<LoggerModel> items = _loggerModelFaker.Generate(2);

            _collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            // Act
            List<LoggerModel> result = (await _repository.QueryAsync()).ToList();

            // Assert
            _collection.Verify(v => v.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()), "FindAsync was not called.");
            Assert.NotNull(result);
            Assert.Equal(items, result);
        }

        #endregion Query

        #region QueryFilter

        /// <summary>
        /// Get filtered data.
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("QueryFilter", "Success")]
        public async Task QueryFilter_FilterId_Data()
        {
            // Arrange
            List<LoggerModel> items = _loggerModelFaker.Generate(1);
            LoggerModel item = items.Single();

            _collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            // Act
            List<LoggerModel> result = (await _repository.QueryAsync(s => s.Id == item.Id)).ToList();

            // Assert
            _collection.Verify(v => v.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()), "FindAsync was not called.");
            Assert.NotNull(result);
            Assert.Equal(items, result);
        }

        #endregion QueryFilter
    }
}