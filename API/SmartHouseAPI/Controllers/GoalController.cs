using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.ViewModel;
using System;
using System.Collections.Generic;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GoalController : ControllerBase
    {
        private readonly IGoalWork<GoalModel> goalWork;

        public GoalController(IGoalWork<GoalModel> gw)
        {
            goalWork = gw;
        }

        // GET: api/getAll
        [HttpGet("getAll", Name = "Goal_All_List")]
        public IEnumerable<GoalModel> GetGoalAll()
        {
            return goalWork.GetGoalAll();
        }

        // GET: api/
        [HttpGet(Name = "Goal_List")]
        public IEnumerable<GoalModel> GetGoals()
        {
            return goalWork.GetGoals();
        }

        // GET: api/{id}
        [HttpGet("{id:guid}", Name = "Goal_Item")]
        public GoalModel GetGoal(Guid id)
        {
            return goalWork.GetGoal(id);
        }

        // POST: api/
        [HttpPost(Name = "Goal_Create")]
        public GoalModel Create(GoalCreateModel data)
        {
            GoalModel result = goalWork.Create(data.Name);
            return result;
        }

        // PUT: api/
        [HttpPut(Name = "Goal_Update")]
        public void Update(GoalModel data)
        {
            goalWork.Update(data);
        }

        // DELETE: api/{id}
        [HttpDelete("{id:guid}", Name = "Goal_Delete")]
        public void Delete(Guid id)
        {
            goalWork.Delete(id);
        }

        // PUT: api/done/{id}
        [HttpPut("done/{id:guid}", Name = "Goal_Done")]
        public void Done(Guid id)
        {
            goalWork.Done(id);
        }

    }
}