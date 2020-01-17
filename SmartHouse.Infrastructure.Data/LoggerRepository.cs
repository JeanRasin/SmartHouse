using MongoDB.Bson;
using MongoDB.Driver;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SmartHouse.Infrastructure.Data
{
    public class LoggerRepository<T> : ILoggerRepository<T> where T : MongoBaseModel
    {
        private readonly LoggerContext dbContext;//todo:remove

        public IMongoCollection<T> Collection { get; private set; }

        public LoggerRepository(LoggerContext dbContext)
        {
            this.dbContext = dbContext;
            Collection = this.dbContext.DbSet<T>();
        }

        public bool Create(T model)
        {
            try
            {
                model.Id = Guid.NewGuid().ToString().Replace("-", "");
                Collection.InsertOne(model);
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public T Find(object id)
        {
            if (!ObjectId.TryParse(id.ToString(), out ObjectId objectId))
            {
                return null;
            }

            var filterId = Builders<T>.Filter.Eq("_id", objectId);
            var model = Collection.Find(filterId).FirstOrDefault();

            return model;
        }

        public IEnumerable<T> Query()
        {
            return Collection.Find(FilterDefinition<T>.Empty).ToList();
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter).ToList();
        }
    }
}
