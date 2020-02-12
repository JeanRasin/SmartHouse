using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.Helpers;
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

        public GoalController(IGoalWork<GoalModel> goalWork)
        {
            this.goalWork = goalWork;
        }

        // GET: api/goal/getAll
        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGoalAll()
        {
            try
            {
                IEnumerable<GoalModel> result = goalWork.GetGoalAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

        // GET: api/goal
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGoals()
        {
            try
            {
                IEnumerable<GoalModel> result = goalWork.GetGoals();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

        // GET: api/goal/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGoal(Guid id)
        {
            try
            {
                GoalModel result = goalWork.GetGoal(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

        // POST: api/goal
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create(GoalCreateInput data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return StatusCode(400);
            }

            try
            {
                GoalModel result = goalWork.Create(data.Name);

                return Ok(result);
            }
            catch (Exception ex) //
            {
                // error log
                return StatusCode(500);
            }

        }

        // PUT: api/goal
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(GoalUpdateInput data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return StatusCode(400);
            }

            try
            {
                goalWork.Update(data.Id, data.Name);

                return StatusCode(200);
            }
            catch (KeyNotFoundException ex)
            {
                // error log
                return NotFound();
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

        // DELETE: api/goal/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                goalWork.Delete(id);

                return StatusCode(200);
            }
            catch (KeyNotFoundException ex)
            {
                // error log
                return NotFound();
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

        // PUT: api/goal/done/
        [HttpPut("done")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Done(GoalDoneInput data)
        {
            try
            {
                goalWork.Done(data.Id, data.Done);

                return StatusCode(200);
            }
            catch (KeyNotFoundException ex)
            {
                // error log
                return NotFound();
            }
            catch (Exception ex)
            {
                // error log
                return StatusCode(500);
            }
        }

    }
}