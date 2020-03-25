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
        private readonly IGoalRepository<GoalModel> _repository;

        public GoalWork(GoalContext context)
        {
            _repository = new GoalRepository(context);
        }

        public GoalWork(IGoalRepository<GoalModel> goalRepository)
        {
            _repository = goalRepository;
        }

        /// <summary>
        /// Get all the goals.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoalModel> GetGoalAll()
        {
            IOrderedEnumerable<GoalModel> result = _repository
                  .GetGoals()
                  .OrderByDescending(p => p.DateUpdate);
            return result;
        }

        /// <summary>
        /// Get outstanding goals.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GoalModel> GetGoals()
        {
            IOrderedEnumerable<GoalModel> result = _repository
                  .GetGoals()
                  .Where(p => p.Done == false)
                  .OrderByDescending(p => p.DateUpdate);

            return result;
        }

        /// <summary>
        /// Get goal by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GoalModel GetGoal(Guid id)
        {
            GoalModel result = _repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            return result;
        }

        /// <summary>
        /// Create a goal.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GoalModel Create(string name)
        {
            var result = new GoalModel(name);

            _repository.Create(result);
            _repository.Save();

            return result;
        }

        /// <summary>
        /// Update goal.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="done"></param>
        public void Update(Guid id, string name, bool done)
        {
            GoalModel result = _repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            result.Name = name;
            result.Done = done;

            _repository.Update(result);
            _repository.Save();
        }

        /// <summary>
        /// Delete goal by id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            GoalModel result = _repository.GetGoal(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            _repository.Remove(id);
            _repository.Save();
        }

        /// <summary>
        /// Mark goal.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="done"></param>
        public void Done(Guid id, bool done)
        {
            GoalModel result = _repository
                 .GetGoals()
                 .Where(p => p.Id == id)
                 .FirstOrDefault();

            if (result == null)
            {
                throw new KeyNotFoundException($"Record id:{id} not found");
            }

            result.Done = done;

            _repository.Update(result);
            _repository.Save();
        }

        #region dispose

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
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