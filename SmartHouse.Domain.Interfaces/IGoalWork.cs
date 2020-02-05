using SmartHouse.Domain.Core;
using System;
using System.Collections.Generic;

namespace SmartHouse.Domain.Interfaces
{
    public interface IGoalWork<T> : IDisposable where T :class
    {
        List<T> GetGoalAll();
        List<T> GetGoals();
        T GetGoal(Guid id);
        void Create(string name);
        void Update(T goal);
        void Delete(Guid id);
        void Done(Guid id);
    }
}
