using System;
using System.Collections.Generic;

namespace SmartHouse.Domain.Interfaces
{
    public interface IGoalRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetGoals();

        T GetGoal(Guid id);

        void Create(T data);

        void Remove(Guid id);

        void Update(T data);

        void Save();
    }
}