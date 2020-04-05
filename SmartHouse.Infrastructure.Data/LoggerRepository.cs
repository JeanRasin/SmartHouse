using MongoDB.Driver;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Domain.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHouse.Infrastructure.Data
{
    /// <summary>
    /// https://gist.github.com/antdimot/5037532
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoggerRepository<T> : ILoggerRepository<T> where T : MongoBase
    {
        public string CategoryName { get; }

        public IMongoCollection<T> Collection { get; private set; }

        public LoggerRepository(ILoggerContext context, string categoryName)
        {
            Collection = context.DbSet<T>();
            CategoryName = categoryName;
        }

        /// <summary>
        /// Create record.
        /// </summary>
        /// <param name="model"></param>
        public void Create(T model)
        {
            model.CategoryName = CategoryName;
            Collection.InsertOne(model);
        }

        /* todo: возможно пригодится.
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
        */

        /// <summary>
        /// Get all entries.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync()
        {
            var result = await Collection.FindAsync<T>(FilterDefinition<T>.Empty).GetAwaiter().GetResult().ToListAsync();
            return result;
        }

        /// <summary>
        /// Filter entries.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> filter)
        {
            var result = await Collection.FindAsync<T>(filter).GetAwaiter().GetResult().ToListAsync();
            return result;
        }
    }
}