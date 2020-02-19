using MongoDB.Driver;
using SmartHouse.Domain.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SmartHouse.Infrastructure.Data
{
    public class LoggerContext : ILoggerContext
    {
        public MongoClient MongoClient { get; set; }

        private readonly IMongoDatabase mongoDb;

        public LoggerContext(string connection, string dbName)
        {
            MongoClient = new MongoClient(connection);
            mongoDb = MongoClient.GetDatabase(dbName);

            // Add test data to the logger table
            List<LoggerModel> dataItems = OnModelCreating() ?? new List<LoggerModel>();

            if (dataItems.Any())
            {
                bool exist = mongoDb.ListCollections().Any();
                if (!exist)
                {
                    InsertDefaultData(mongoDb, dataItems);
                }
            }
        }

        public virtual IMongoCollection<T> DbSet<T>() where T : MongoBaseModel
        {
            string tableName = GetTableName<T>();
            IMongoCollection<T> result = mongoDb.GetCollection<T>(tableName);
            return result;
        }

        public virtual List<LoggerModel> OnModelCreating()
        {
            return null;
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
