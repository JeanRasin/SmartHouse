using MongoDB.Driver;
using SmartHouse.Domain.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SmartHouse.Infrastructure.Data
{
    public interface ILoggerContext
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

        public LoggerContext(string connection, string dbName)
        {
            mongoClient = new MongoClient(connection);
            mongoDb = mongoClient.GetDatabase(dbName);
        }

        public virtual IMongoCollection<T> DbSet<T>() where T : MongoBaseModel
        {
            var tableName = typeof(T).GetCustomAttribute<TableAttribute>(false).Name;
            var result = mongoDb.GetCollection<T>(tableName);
            return result;
        }
    }
}
