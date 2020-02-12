using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public LoggerController(ILoggerWork loggerWork)
        {
            this.loggerWork = loggerWork;
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

    }
}