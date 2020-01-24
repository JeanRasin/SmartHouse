using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHouse.Domain.Interfaces
{
    public interface ILoggerRepository<T> where T : class
    {
       // T Find(object id);
        bool Create(T model);
        Task<IEnumerable<T>> QueryAsync();
        Task<List<T>> QueryAsync(Expression<Func<T, bool>> filter);
       // Task<List<T>> QueryAsync(FilterDefinition<T> filter);
    }
}
