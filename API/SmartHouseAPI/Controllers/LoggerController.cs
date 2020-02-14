using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.Helpers;
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
            IEnumerable<LoggerModel> result = await loggerWork.GetLoggerAsync();

            if (result == null)
            {

                throw new NotFoundException("Logger result is null.");
            }

            return Ok(result);
        }

    }
}