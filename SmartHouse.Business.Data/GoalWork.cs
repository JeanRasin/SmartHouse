﻿using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHouse.Business.Data
{
    public class GoalWork : IGoalWork<GoalModel> 
    {
        private readonly IGoalRepository<GoalModel> repository;

        public GoalWork(GoalContext context)
        {
            repository = new GoalRepository(context);
        }

        public GoalWork(IGoalRepository<GoalModel> goalRepository)
        {
            repository = goalRepository;
        }

        public IEnumerable<GoalModel> GetGoalAll()
        {
            IOrderedEnumerable<GoalModel> result = repository
                  .GetGoals()
                  .OrderByDescending(p => p.DateUpdate);
            return result;
        }

        public IEnumerable<GoalModel> GetGoals()
        {
            IOrderedEnumerable<GoalModel> result = repository
                  .GetGoals()
                  .Where(p => p.Done == false)
                  .OrderByDescending(p => p.DateUpdate);

            return result;
        }

        public GoalModel GetGoal(Guid id)
        {
            GoalModel result = repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            return result;
        }

        public GoalModel Create(string name)
        {
            var result = new GoalModel(name);

            repository.Create(result);
            repository.Save();

            return result;
        }

        public void Update(Guid id, string name)
        {
            GoalModel result = repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            result.Name = name;

            repository.Update(result);
            repository.Save();
        }

        public void Delete(Guid id)
        {
            GoalModel result = repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            repository.Remove(id);
            repository.Save();
        }

        public void Done(Guid id, bool done)
        {
            GoalModel result = repository
                 .GetGoals()
                 .Where(p => p.Id == id)
                 .FirstOrDefault();

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            result.Done = done;

            repository.Update(result);
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
