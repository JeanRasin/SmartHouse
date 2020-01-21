using System;
using System.Collections.Generic;
using SmartHouse.Domain.Core;

namespace SmartHouse.Domain.Interfaces
{
    public interface IGoalRepository : IDisposable
    {
        IEnumerable<GoalModel> GetGoals();
        void Save();
    }
}
