using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using SmartHouse.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryTest
{
    public class HeirLoggerContext : LoggerContext
    {
        //public LoggerContext(string connection, string dbName) : base(connection, dbName)
        public HeirLoggerContext(MongoSettings configuration) : base(new MongoClient(configuration.Connection), configuration.DatabaseName)
        {
        }

        public override List<LoggerModel> OnModelCreating()
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

            return loggerModelFaker.Generate(10);
        }
    }

    [Collection("Logger context")]
    public class LoggerContextTest
    {
        // private Mock<IOptions<Mongosettings>> _mockOptions;

       // private Mock<MongoSettings> _mockOptions;

        private Mock<IMongoDatabase> _mockDB;

        private Mock<IMongoClient> _mockClient;

        public LoggerContextTest()
        {
           // _mockOptions = new Mock<IOptions<Mongosettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            var resultCommand = new BsonDocument("ok", 1);
            _mockDB.Setup(stub => stub.RunCommandAsync<BsonDocument>(It.IsAny<Command<BsonDocument>>(), It.IsAny<ReadPreference>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultCommand)
                .Verifiable();


            _mockClient = new Mock<IMongoClient>();
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

            return loggerModelFaker.Generate(10);
        }

        [Fact]
        public void LoggerContext_Constructor_Success()
        {
            // Arrange
            var settings = new MongoSettings()
            {
                Connection = "mongodb://tes123 ",
                DatabaseName = "TestDB"
            };

            _mockClient.Setup(c => c.GetDatabase(settings.DatabaseName, null)).Returns(_mockDB.Object).Verifiable();

            //Act 
            var context = new LoggerContext(_mockClient.Object, settings.DatabaseName);

            //Assert 
            Assert.NotNull(context);
            _mockClient.Verify(v => v.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()), Times.Once);
        }

        [Fact]
        public async void LoggerContext_DbSet_Success()
        {
            var settings = new MongoSettings()
            {
                Connection = "mongodb://tes123 ",
                DatabaseName = "TestDB"
            };

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

            var loggerCollectionMock = new Mock<IMongoCollection<LoggerModel>>();

            Mock<IAsyncCursor<LoggerModel>> loggerCursor = new Mock<IAsyncCursor<LoggerModel>>();
            loggerCursor.Setup(s => s.Current).Returns(loggerList);
            loggerCursor.SetupSequence(s => s.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            loggerCursor.SetupSequence(s => s.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                 .Returns(Task.FromResult(false));

            loggerCollectionMock.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(),
            It.IsAny<FindOptions<LoggerModel, LoggerModel>>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(loggerCursor.Object);

            _mockDB.Setup(c => c.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(loggerCollectionMock.Object).Verifiable();
            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_mockDB.Object);
            var context = new LoggerContext(_mockClient.Object, settings.DatabaseName);

            //Act 
            IMongoCollection<LoggerModel> collection = context.DbSet<LoggerModel>();
            List<LoggerModel> result = await collection.FindAsync<LoggerModel>(FilterDefinition<LoggerModel>.Empty).GetAwaiter().GetResult().ToListAsync();

            //Assert 
            Assert.Equal(result, loggerList);
            _mockDB.Verify(v => v.GetCollection<LoggerModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()));
        }

        [Fact]
        public void LoggerContext_OnModelCreating_Success()
        {
            // Arrange
            var settings = new MongoSettings()
            {
                Connection = "mongodb://tes123 ",
                DatabaseName = "TestDB"
            };

            _mockClient.Setup(c => c.GetDatabase(settings.DatabaseName, null)).Returns(_mockDB.Object).Verifiable();
            var context = new LoggerContext(_mockClient.Object, settings.DatabaseName);

            //Act 
            List<LoggerModel> result = context.OnModelCreating();

            //Assert 
            Assert.Null(result);
        }

        /*
        [Fact(Skip = "Возможно и не надо.")] // todo:!!!
        public void GetTableName_AttributeName()
        {
            //var loggerContext = new LoggerContext("","");
            Type type = typeof(LoggerContext);
            var hello = Activator.CreateInstance(type, "mongodb://localhost", "dbname");
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "GetTableName" && x.IsPrivate)
                .First();

            var getTableName = (string)method.Invoke(hello, new object[] { });

            //getTableName.
        }
        */
    }
}