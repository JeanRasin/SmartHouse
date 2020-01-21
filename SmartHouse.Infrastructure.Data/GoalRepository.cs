using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalRepository : IGoalRepository
    {
        private readonly GoalContext db;

        public GoalRepository(GoalContext context)
        {
            db = context;
        }

        public IEnumerable<GoalModel> GetGoals()
        {
            return db.Goals.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        #region dispose
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
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
