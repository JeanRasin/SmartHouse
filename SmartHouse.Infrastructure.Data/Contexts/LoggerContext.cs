using MongoDB.Driver;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces.Contexts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SmartHouse.Infrastructure.Data
{
    public class LoggerContext : ILoggerContext
    {
        private readonly IMongoClient _mongoClient;

        private readonly IMongoDatabase _mongoDb;

        public LoggerContext(IMongoClient mongoClient, string databaseName)
        {
            _mongoClient = mongoClient;
            _mongoDb = _mongoClient.GetDatabase(databaseName);
        }

        public virtual IMongoCollection<T> DbSet<T>() where T : MongoBase
        {
            string tableName = GetTableName<T>();
            IMongoCollection<T> result = _mongoDb.GetCollection<T>(tableName);
            return result;
        }

        /// <summary>
        /// Override and set initial values
        /// </summary>
        /// <returns></returns>
        public virtual List<Logger> OnModelCreating()
        {
            return null;
        }

        /// <summary>
        /// Add initial values to table if table is empty
        /// </summary>
        public void EnsureCreated()
        {
            // Add test data to the logger table
            List<Logger> dataItems = OnModelCreating() ?? new List<Logger>();

            if (dataItems.Any())
            {
                bool exist = _mongoDb.ListCollections().Any();
                if (!exist)
                {
                    InsertDefaultData(_mongoDb, dataItems);
                }
            }
        }

        /// <summary>
        /// Get table name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetTableName<T>()
        {
            return typeof(T).GetCustomAttribute<TableAttribute>(false).Name;
        }

        /// <summary>
        /// Add initial values to the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mongoDb"></param>
        /// <param name="items"></param>
        private void InsertDefaultData<T>(IMongoDatabase mongoDb, List<T> items)
        {
            var tableName = GetTableName<T>();
            var loggerModel = mongoDb.GetCollection<T>(tableName);

            foreach (var item in items)
            {
                loggerModel.InsertOne(item);
            }
        }
    }
}