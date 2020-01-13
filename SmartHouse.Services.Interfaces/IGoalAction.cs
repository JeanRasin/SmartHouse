using System;
using SmartHouse.Domain.Core;

namespace SmartHouse.Services.Interfaces
{
    public interface IGoalAction
    {
        void Action(Goal goal);
    }
}
