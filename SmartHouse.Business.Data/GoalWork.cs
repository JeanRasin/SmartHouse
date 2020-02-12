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
            IOrderedEnumerable<GoalModel> items = repository
                  .GetGoals()
                  .OrderByDescending(p => p.DateUpdate);
            return items;
        }

        public IEnumerable<GoalModel> GetGoals()
        {
            IOrderedEnumerable<GoalModel> items = repository
                  .GetGoals()
                  .Where(p => p.Done == false)
                  .OrderByDescending(p => p.DateUpdate);

            return items;
        }

        public GoalModel GetGoal(Guid id)
        {
            GoalModel item = repository.GetGoal(id);
            return item;
        }

        public GoalModel Create(string name)
        {
            var item = new GoalModel(name);
            repository.Create(item);
            repository.Save();

            return item;
        }

        public void Update(Guid id, string name)
        {
            GoalModel item = repository.GetGoal(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            item.Name = name;

            repository.Update(item);
            repository.Save();
        }

        public void Delete(Guid id)
        {
            GoalModel item = repository.GetGoal(id);

            if (item == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            repository.Remove(id);
            repository.Save();
        }

        public void Done(Guid id, bool done)
        {
            GoalModel item = repository
                 .GetGoals()
                 .Where(p => p.Id == id)
                 .FirstOrDefault();

            if (item == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            item.Done = done;

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
