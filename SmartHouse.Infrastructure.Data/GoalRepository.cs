using Microsoft.EntityFrameworkCore;
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
            GoalModel result = db.Goals.Find(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record with id:{id} not found");
            }

            return result;
        }

        public void Create(GoalModel data)
        {
            db.Entry(data).State = EntityState.Added;
        }

        public void Remove(Guid id)
        {
            var goal = db.Goals.Find(id);
            if (goal != null)
            {
                db.Entry(goal).State = EntityState.Deleted;
            }
            else
            {
                throw new KeyNotFoundException($"Record with id:{id} not found");
            }
        }

        public void Update(GoalModel data)
        {
            var goal = db.Goals.Find(data.Id);
            if (goal != null)
            {
                db.Entry(data).State = EntityState.Modified;
            }
            else
            {
                throw new KeyNotFoundException($"Record with id:{data.Id} not found");
            }
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
