using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.Helpers;
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
        private readonly ILogger log;

        public GoalController(IGoalWork<GoalModel> goalWork, ILogger log)
        {
            this.goalWork = goalWork;
            this.log = log;
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
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                    log.LogError($"Goal object id:{id} not found.");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/goal
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create(GoalCreateInput data)
        {
            if (!ModelState.IsValid)
            {
                log.LogError("Goal model is not valid.");
                return BadRequest(ModelState);
            }

            try
            {
                GoalModel result = goalWork.Create(data.Name);
                return Created(Url.RouteUrl(result.Id), result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/goal
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(GoalUpdateInput data)
        {
            if (!ModelState.IsValid)
            {
                log.LogError("Goal model is not valid.");
                return BadRequest(ModelState);
            }

            try
            {
                goalWork.Update(data.Id, data.Name);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                log.LogError($"Goal object id:{data.Id} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/goal/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                goalWork.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                log.LogError($"Goal object id:{id} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/goal/done/
        [HttpPut("done")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Done(GoalDoneInput data)
        {
            try
            {
                goalWork.Done(data.Id, data.Done);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                log.LogError($"Goal object id:{data.Id} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}