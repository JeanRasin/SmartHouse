using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.ApiException;
using SmartHouseAPI.InputModel;
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
            IEnumerable<GoalModel> result = goalWork.GetGoalAll();
            return Ok(result);
        }

        // GET: api/goal
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGoals()
        {
            IEnumerable<GoalModel> result = goalWork.GetGoals();
            return Ok(result);
        }

        // GET: api/goal/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGoal(Guid id)
        {
            GoalModel result = goalWork.GetGoal(id);

            if (result == null)
            {
                throw new NotFoundException($"Goal object id:{id} not found.");
            }

            return Ok(result);
        }

        // POST: api/goal
        /// <summary>
        /// Create goal.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "name": "Item1",
        ///     }
        ///
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       public IActionResult Create([FromBody]GoalCreateInput data)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException("Goal model is not valid.", ModelState);
            }

            GoalModel result = goalWork.Create(data.Name);
            return Created(Url.RouteUrl(result.Id), result);
        }

        // PUT: api/goal
        /// <summary>
        /// Update goal.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT
        ///     {
        ///        "id": "c55940df-c000-c450-1601-5e9aa3ed8bbc",
        ///        "name": "Item1",
        ///     }
        ///
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(GoalUpdateInput), StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody]GoalUpdateInput data)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException("Goal model is not valid.", ModelState);
            }

            try
            {
                goalWork.Update(data.Id, data.Name);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException($"Goal object id:{data.Id} not found.");
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
                throw new NotFoundException($"Goal object id:{id} not found.");
            }
        }

        // PUT: api/goal/done/
        /// <summary>
        /// Done goal.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT 
        ///     {
        ///        "id": "c55940df-c000-c450-1601-5e9aa3ed8bbc",
        ///        "done": "true",
        ///     }
        ///
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("done")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Done([FromBody]GoalDoneInput data)
        {
            try
            {
                goalWork.Done(data.Id, data.Done);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException($"Goal object id:{data.Id} not found.");
            }
        }
    }
}