using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;

namespace SmartHouse.Infrastructure.Data
{
   public class GoalRepository : IGoalRepository
    {
        private ActContext db;

        public GoalRepository(ActContext context)
        {
            db = context;
        }

        public IEnumerable<Goal> GetGoals()
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
