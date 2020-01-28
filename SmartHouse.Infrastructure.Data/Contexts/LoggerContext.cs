using Bogus;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartHouse.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartHouse.Infrastructure.Data
{
    public interface ILoggerContext//todo: перенести в другое место
    {
        IMongoCollection<T> DbSet<T>() where T : MongoBaseModel;
    }

    public class LoggerContext : ILoggerContext
    {
        public readonly MongoClient mongoClient;
        public readonly IMongoDatabase mongoDb;

        public LoggerContext()//todo:не понятно как использовать для Moq
        {
        }

        public LoggerContext(string connection, string dbName) : this()
        {
            mongoClient = new MongoClient(connection);
            mongoDb = mongoClient.GetDatabase(dbName);

#if (DEBUG)

            bool exist = mongoDb.ListCollections().Any();
            if (!exist)
            {
                // Add test data to the logger table
                InsertDefaultData(mongoDb, CreateDefaulLoggerModeltData(12));
            }
#endif
        }

        public virtual IMongoCollection<T> DbSet<T>() where T : MongoBaseModel
        {
            var tableName = GetTableName<T>();
            var result = mongoDb.GetCollection<T>(tableName);
            return result;
        }

        private List<LoggerModel> CreateDefaulLoggerModeltData(int count = 10)
        {
            // Randomizer.Seed = new Random(544);

            var eventIdFaker = new Faker<EventId>()
                 .RuleFor(o => o.StateId, f => f.Random.Int(1, 10))
                 .RuleFor(o => o.Name, f => f.Random.String2(10));

            var loggerModelFaker = new Faker<LoggerModel>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid().ToString("N"))
                 .RuleFor(o => o.EventId, f => eventIdFaker.Generate())
                 .RuleFor(o => o.LogLevel, f => f.PickRandom<Microsoft.Extensions.Logging.LogLevel>())
                 .RuleFor(o => o.Message, f => f.Random.Words())
                 .RuleFor(o => o.Date, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)));

            List<LoggerModel> result = loggerModelFaker.Generate(count);
            return result;
        }

        private void InsertDefaultData<T>(IMongoDatabase mongoDb, List<T> items)
        {
            var tableName = GetTableName<T>();
            var loggerModel = mongoDb.GetCollection<T>(tableName);

            foreach (var item in items)
            {
                loggerModel.InsertOne(item);
            }
        }

        private string GetTableName<T>()
        {
            return typeof(T).GetCustomAttribute<TableAttribute>(false).Name;
        }
    }
}
