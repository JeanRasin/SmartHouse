using MongoDB.Driver;
using SmartHouse.Domain.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SmartHouse.Infrastructure.Data
{
    public class LoggerContext
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase mongoDb;

        public LoggerContext(string connection, string dbName)
        {
            mongoClient = new MongoClient(connection);
            mongoDb = mongoClient.GetDatabase(dbName);
        }

        public IMongoCollection<T> DbSet<T>() where T : MongoBaseModel
        {
            var tableName = typeof(T).GetCustomAttribute<TableAttribute>(false).Name;
            var result = mongoDb.GetCollection<T>(tableName);
            return result;
        }
    }
}
