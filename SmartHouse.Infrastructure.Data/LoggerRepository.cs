﻿using MongoDB.Bson;
using MongoDB.Driver;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHouse.Infrastructure.Data
{
    public class LoggerRepository<T> : ILoggerRepository<T> where T : MongoBaseModel
    {
        public IMongoCollection<T> Collection { get; private set; }

        public LoggerRepository(ILoggerContext context)
        {
            Collection = context.DbSet<T>();
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

        //public T Find(object id)
        //{
        //    if (!ObjectId.TryParse(id.ToString(), out ObjectId objectId))
        //    {
        //        return null;
        //    }

        //    var filterId = Builders<T>.Filter.Eq("_id", objectId);
        //    var model = Collection.Find(filterId).FirstOrDefault();

        //    return model;
        //}

        public async Task<IEnumerable<T>> QueryAsync()
        {
            var result = await Collection.FindAsync<T>(FilterDefinition<T>.Empty).GetAwaiter().GetResult().ToListAsync();
            return result;
        }

          public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter)
     // public async Task<List<T>> QueryAsync(FilterDefinition<T> filter) //, FindOptions<T, TProjection> options = null
        {
            return await Collection.FindAsync<T>(filter).GetAwaiter().GetResult().ToListAsync();
        }
    }
}
