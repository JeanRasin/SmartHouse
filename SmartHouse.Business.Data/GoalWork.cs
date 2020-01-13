using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHouse.Business.Data
{
   public interface IGoalWork
    {
        List<Goal> GetGoals();
    }

    public class GoalWork : IGoalWork
    {
        private readonly ActContext db;

        public GoalWork(ActContext context)
        {
            db = context;
        }

        public List<Goal> GetGoals()
        {
            return db.Goals.ToList();
        }
    }
}
