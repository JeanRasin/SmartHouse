using MongoDB.Driver;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using RepositoryTest.Helpers;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Threading;

namespace RepositoryTest
{
    public class LoggerRepositoryTest
    {
        public LoggerRepositoryTest()
        {

        }

        [Fact]
        public void Repository_InsertOne_void()
        {
            var data = new LoggerModel
            {
                Id = "1",
                Message = "test"
            };

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
            var items = new List<LoggerModel> {
            new LoggerModel
            {
                Id = "1",
                Message = "test 1"
            },
            new LoggerModel
            {
                Id = "2",
                Message = "test 2"
            }};

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
            var items = new List<LoggerModel> {
            new LoggerModel
            {
                Id = "1",
                Message = "test 1"
            },
            new LoggerModel
            {
                Id = "2",
                Message = "test 2"
            }};

            //Expression<Func<LoggerModel, bool>> filter = s => s.Message == "test 2";

            var collection = new Mock<IMongoCollection<LoggerModel>>();
            collection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<LoggerModel>>(), It.IsAny<FindOptions<LoggerModel, LoggerModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MockAsyncCursor<LoggerModel>(items));

            var context = new Mock<LoggerContext>();
            context.Setup(l => l.DbSet<LoggerModel>()).Returns(collection.Object);

            var repository = new LoggerRepository<LoggerModel>(context.Object);

            // var hh = new Expression<Func<LoggerModel, bool>>();

            // Func<LoggerModel, bool> func = l => true;
            //Expression<Func<LoggerModel, bool>> el = e => GetHHH(e);

            //FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;


            //FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            //{
            //    BatchSize = 2,
            //    NoCursorTimeout = false
            //};

            // var filter = Builders<LoggerModel>.Filter.Eq("Name", "Bill");

            var query = repository.QueryAsync(s => s.Message == "test 2");

            //var result = hh.ToList();

            // List<LoggerModel> result = repository.QueryAsync(filter).Result.ToList();

            Assert.NotNull(query);
            Assert.Equal(items, query.Result);
        }
    }
}
