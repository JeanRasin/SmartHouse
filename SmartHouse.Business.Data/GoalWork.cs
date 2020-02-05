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
        public List<GoalModel> GetGoalAll()
        {
            return repository
                  .GetGoals()
                  .OrderByDescending(p=>p.DateUpdate)
                  .ToList();
        }

        public List<GoalModel> GetGoals()
        {
            return repository
                  .GetGoals()
                  .Where(p => p.Done == false)
                  .OrderByDescending(p => p.DateUpdate)
                  .ToList();
        }

        public GoalModel GetGoal(Guid id)
        {
            //GoalModel item = repository
            //      .GetGoals()
            //      .Where(p => p.Id == id)
            //      .Single();

            return repository.GetGoal(id);
        }

        public void Create(string name)
        {
            var item = new GoalModel(name);
            repository.Create(item);
            repository.Save();
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
