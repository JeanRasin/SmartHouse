﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouse.Infrastructure.Data;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        //private readonly IGoalRepository repo;

        private readonly ActContext db;
        private readonly IGoalWork goalWork;

        public GoalController(ActContext context, IGoalWork gw)
        {
            db = context;
            goalWork = gw;

           //  repo = new GoalRepository();
        }

        [HttpGet("get")]
        public IEnumerable<Goal> GetGoals()
        {
            //return db.Goals.ToList();

            return goalWork.GetGoals();
        }

    }
}