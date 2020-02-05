using MongoDB.Driver;
using SmartHouse.Domain.Core;

namespace SmartHouse.Infrastructure.Data
{
    public interface ILoggerContext
    {
        IMongoCollection<T> DbSet<T>() where T : MongoBaseModel;
    }
}
