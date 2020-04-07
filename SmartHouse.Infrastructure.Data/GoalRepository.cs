using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalRepository : IGoalRepository<Goal>
    {
        private readonly GoalContext _db;

        public GoalRepository(GoalContext context)
        {
            _db = context;
        }

        public IEnumerable<Goal> GetGoals()
        {
            return _db.Goals;
        }

        /// <summary>
        /// Get goal by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Goal GetGoal(Guid id)
        {
            Goal result = _db.Goals.Find(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record with id:{id} not found");
            }

            return result;
        }

        /// <summary>
        /// Create goal.
        /// </summary>
        /// <param name="data"></param>
        public void Create(Goal data)
        {
            _db.Entry(data).State = EntityState.Added;
        }

        /// <summary>
        /// Remove goal by id.
        /// </summary>
        /// <param name="id"></param>
        public void Remove(Guid id)
        {
            var goal = _db.Goals.Find(id);
            if (goal != null)
            {
                _db.Entry(goal).State = EntityState.Deleted;
            }
            else
            {
                throw new KeyNotFoundException($"Record with id:{id} not found");
            }
        }

        /// <summary>
        /// Update goal.
        /// </summary>
        /// <param name="data"></param>
        public void Update(Goal data)
        {
            var goal = _db.Goals.Find(data.Id);
            if (goal != null)
            {
                _db.Entry(data).State = EntityState.Modified;
            }
            else
            {
                throw new KeyNotFoundException($"Record with id:{data.Id} not found");
            }
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }

        #region dispose

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion dispose
    }
}