using MongoDB.Driver;
using SmartHouse.Domain.Core;

namespace SmartHouse.Infrastructure.Data
{
    public interface ILoggerContext//todo: перенести в другое место
    {
        IMongoCollection<T> DbSet<T>() where T : MongoBaseModel;
    }
}
