using Bogus;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace RepositoryTest
{
    [Collection("Logger context")]
    public class LoggerContextTest
    {
        private static readonly List<LoggerModel> _testDataItems = GetTestData();

        private static readonly MongoSettings _settings = new MongoSettings()
        {
            Connection = "mongodb://tes123 ",
            DatabaseName = "TestDB"
        };

        private readonly Mock<IMongoDatabase> _mockDB;

        private readonly Mock<IMongoClient> _mockClient;

        public LoggerContextTest()
        {
            _mockDB = new Mock<IMongoDatabase>();
            var resultCommand = new BsonDocument("ok", 1);
            _mockDB.Setup(stub => stub.RunCommandAsync<BsonDocument>(It.IsAny<Command<BsonDocument>>(), It.IsAny<ReadPreference>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultCommand)
                .Verifiable();

            _mockClient = new Mock<IMongoClient>();

            var loggerCollectionMock = new Mock<IMongoCollection<LoggerModel>>();
        }

        /// <summary>
        /// Inherited LoggerContext class with initial data.
        /// </summary>
        private class HeirLoggerContext : LoggerContext
        {
            public HeirLoggerContext(IMongoClient mongoClient, string databaseName) : base(mongoClient, databaseName)
            {
            }

            public override List<LoggerModel> OnModelCreating()
            {
                var eventIdFaker = new Faker<EventId>()
                 .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                 .RuleFor(o => o.Name, f => f.Random.String2(10));

                var loggerModelFaker = new Faker<LoggerModel>()
                     .StrictMode(true)
                     .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                     .RuleFor(o => o.CategoryName, f => "Category test")
                     .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                     .RuleFor(o => o.LogLevel, f => f.PickRandom<Microsoft.Extensions.Logging.LogLevel>())
                     .RuleFor(o => o.Message, f => f.Random.Words())
                     .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

                return loggerModelFaker.Generate(10);
            }
        }

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static List<LoggerModel> GetTestData(int n = 10)
        {
            var eventIdFaker = new Faker<SmartHouse.Domain.Core.EventId>()
             .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
             .RuleFor(o => o.Name, f => f.Random.String2(10));

            var loggerModelFaker = new Faker<LoggerModel>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                 .RuleFor(o => o.CategoryName, f => "Category test")
                 .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                 .RuleFor(o => o.LogLevel, f => f.PickRandom<Microsoft.Extensions.Logging.LogLevel>())
                 .RuleFor(o => o.Message, f => f.Random.Words())
                 .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

            return loggerModelFaker.Generate(n);
        }

        #region Constructor

        [Fact]
        [Trait("Constructor", "Success")]
        public void LoggerContext_Constructor_Success()
        {
            // Arrange
            _mockClient.Setup(c => c.GetDatabase(_settings.DatabaseName, null)).Returns(_mockDB.Object).Verifiable();

            //Act
            var context = new LoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Assert
            Assert.NotNull(context);
            _mockClient.Verify(v => v.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()), Times.Once);
        }

        #endregion Constructor

        #region DbSet

        [Fact]
        [Trait("DbSet", "Success")]
        public void LoggerContext_DbSet_Success()
        {
            // Arrange
            var loggerCollectionMock = new Mock<IMongoCollection<LoggerModel>>();
            _mockDB.Setup(c => c.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(loggerCollectionMock.Object).Verifiable();
            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_mockDB.Object);
            var context = new LoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Act
            IMongoCollection<LoggerModel> collection = context.DbSet<LoggerModel>();

            //Assert
            Assert.NotNull(collection);
            _mockDB.Verify(v => v.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()), Times.Once);
        }

        #endregion DbSet

        #region OnModelCreating

        [Fact]
        [Trait("Call OnModelCreating", "Result Null")]
        public void LoggerContext_OnModelCreating_Success()
        {
            // Arrange
            _mockClient.Setup(c => c.GetDatabase(_settings.DatabaseName, null)).Returns(_mockDB.Object).Verifiable();
            var context = new LoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Act
            List<LoggerModel> result = context.OnModelCreating();

            //Assert
            Assert.Null(result);
        }

        #endregion OnModelCreating

        #region EnsureCreatedOn

        [Fact]
        [Trait("Call EnsureCreated", "OnModelCreating Null Data")]
        public void LoggerContext_EnsureCreatedOnModelCreatingNull_Success()
        {
            // Arrange
            _mockDB.Setup(c => c.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>())).Verifiable();
            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_mockDB.Object);
            var context = new LoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Act
            context.EnsureCreated();

            //Assert
            _mockDB.Verify(v => v.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        [Trait("Call EnsureCreated", "List Collections Is Full")]
        public void LoggerContext_EnsureCreatedListCollectionsNotNull_Success()
        {
            // Arrange
            var loggerCollectionMock = new Mock<IMongoCollection<LoggerModel>>();
            var bsonDocumentItems = new List<BsonDocument>
            {
                new BsonDocument("key", "test")
            };

            var bsonCursor = new Mock<IAsyncCursor<BsonDocument>>();
            bsonCursor.Setup(s => s.Current).Returns(bsonDocumentItems);
            bsonCursor.SetupSequence(s => s.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            _mockDB.Setup(c => c.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>())).Returns(bsonCursor.Object).Verifiable();
            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_mockDB.Object);
            var context = new HeirLoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Act
            context.EnsureCreated();

            //Assert
            _mockDB.Verify(v => v.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            loggerCollectionMock.Verify(v => v.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        [Trait("Call EnsureCreated", "Insert")]
        public void LoggerContext_EnsureCreatedListCollectionsNull_Success()
        {
            // Arrange
            var loggerCollectionMock = new Mock<IMongoCollection<LoggerModel>>();
            var bsonCursorMock = new Mock<IAsyncCursor<BsonDocument>>();

            _mockDB.Setup(c => c.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(loggerCollectionMock.Object).Verifiable();
            _mockDB.Setup(c => c.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>())).Returns(bsonCursorMock.Object).Verifiable();

            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_mockDB.Object);
            var context = new HeirLoggerContext(_mockClient.Object, _settings.DatabaseName);

            //Act
            context.EnsureCreated();

            //Assert
            _mockDB.Verify(v => v.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()), Times.Once);
            _mockDB.Verify(v => v.ListCollections(It.IsAny<ListCollectionsOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            loggerCollectionMock.Verify(v => v.InsertOne(It.IsAny<LoggerModel>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Exactly(_testDataItems.Count()));
        }

        #endregion EnsureCreatedOn
    }
}