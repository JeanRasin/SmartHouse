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

    public class GoalWork : IGoalWork, IDisposable
    {
        private readonly GoalRepository repository;

        public GoalWork(GoalContext context)
        {
            repository = new GoalRepository(context);
        }

        public List<Goal> GetGoals()
        {
            return repository.GetGoals().ToList();
        }

        #region dispose
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    repository.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
