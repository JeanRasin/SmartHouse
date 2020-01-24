using SmartHouse.Domain.Core;

namespace SmartHouse.Services.Interfaces
{
    public interface IGoalAction
    {
        void Action(GoalModel goal);
    }
}
