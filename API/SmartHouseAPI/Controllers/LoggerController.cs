using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.ApiException;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILoggerWork loggerWork;

        public LoggerController(ILoggerWork loggerWork)
        {
            this.loggerWork = loggerWork;
        }

        /// <summary>
        /// Get log a data.
        /// </summary>
        /// <returns>Returns list logs</returns>
        /// <response code="200">Returns list logs</response>
        /// <response code="404">Not data</response>
        /// <response code="500">Internal server error</response>  
        // GET api/logger
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggerAsync()
        {
            IEnumerable<LoggerModel> result = await loggerWork.GetLoggerAsync();//todo:remove

            if (result == null)
            {
                throw new NotFoundException("Logger result is null.");
            }

            return Ok(result);
        }
    }
}