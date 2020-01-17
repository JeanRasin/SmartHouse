using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmartHouse.Domain.Interfaces.Logger
{
    public interface ILoggerRepository<T> where T : class
    {
        T Find(object id);
        bool Create(T model);
        IEnumerable<T> Query();
        IEnumerable<T> Query(Expression<Func<T, bool>> filter);
    }
}
