using SmartHouse.Domain.Core;
using System.Collections.Generic;

namespace SmartHouse.Domain.Interfaces
{
    public interface IGoalWork
    {
        List<GoalModel> GetGoals();
    }
}
