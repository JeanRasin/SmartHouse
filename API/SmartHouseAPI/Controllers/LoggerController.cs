using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Domain.Core;
using SmartHouse.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILoggerWork _loggerWork;

        public LoggerController(ILoggerWork loggerWork)
        {
            _loggerWork = loggerWork;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggerAsync()
        {
            IEnumerable<Logger> result = await _loggerWork.GetLoggerAsync();
            return Ok(result);
        }
    }
}