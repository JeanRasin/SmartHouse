using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILoggerWork loggerWork;
        private readonly ILogger log;

        public LoggerController(ILoggerWork loggerWork, ILogger log)
        {
            this.loggerWork = loggerWork;
            this.log = log;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggerAsync()
        {
            try
            {
                IEnumerable<LoggerModel> result = await loggerWork.GetLoggerAsync();

                if (result == null)
                {
                    log.LogError("Logger result null.");
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

    }
}