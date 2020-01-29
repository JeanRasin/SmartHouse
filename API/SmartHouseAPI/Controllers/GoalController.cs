using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System.Collections.Generic;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalWork goalWork;

        public GoalController(IGoalWork gw)
        {
            goalWork = gw;
        }

        [HttpGet]
        public IEnumerable<GoalModel> GetGoals()
        {
            return goalWork.GetGoals();
        }
    }
}