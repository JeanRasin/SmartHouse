using MongoDB.Driver;
using SmartHouse.Domain.Core;

namespace SmartHouse.Domain.Interfaces.Contexts
{
    public interface ILoggerContext
    {
        IMongoCollection<T> DbSet<T>() where T : MongoBase;

        void EnsureCreated();
    }
}