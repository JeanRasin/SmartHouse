using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalRepository : IGoalRepository<GoalModel>
    {
        private readonly GoalContext db;

        public GoalRepository(GoalContext context)
        {
            db = context;
        }

        public IEnumerable<GoalModel> GetGoals()
        {
            return db.Goals;
        }

        public GoalModel GetGoal(Guid id)
        {
            return db.Goals.Find(id);
        }

        public void Create(GoalModel data)
        {
            db.Goals.Add(data);
        }

        public void Remove(Guid id)
        {
            var goal = db.Goals.Find(id);
            if (goal != null)
                db.Remove(goal);
        }

        public void Update(GoalModel data)
        {
            db.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
