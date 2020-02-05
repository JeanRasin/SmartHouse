using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHouse.Business.Data
{
    public class GoalWork : IGoalWork<GoalModel>
    {
        private readonly GoalRepository repository;

        public GoalWork(GoalContext context)
        {
            repository = new GoalRepository(context);
        }
        public IEnumerable<GoalModel> GetGoalAll()
        {
            var items = repository
                  .GetGoals()
                  .OrderByDescending(p => p.DateUpdate);
            return items;
        }

        public IEnumerable<GoalModel> GetGoals()
        {
            var items = repository
                  .GetGoals()
                  .Where(p => p.Done == false)
                  .OrderByDescending(p => p.DateUpdate);

            return items;
        }

        public GoalModel GetGoal(Guid id)
        {
            var item = repository.GetGoal(id);
            return item;
        }

        public GoalModel Create(string name)
        {
            var item = new GoalModel(name);
            repository.Create(item);
            repository.Save();

            return item;
        }

        public void Update(GoalModel goal)
        {
            repository.Update(goal);
            repository.Save();
        }

        public void Delete(Guid id)
        {
            repository.Remove(id);
            repository.Save();
        }

        public void Done(Guid id)
        {
            GoalModel item = repository
                 .GetGoals()
                 .Where(p => p.Id == id)
                 .Single();

            item.Done = true;

            repository.Update(item);
            repository.Save();
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
