using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Infrastructure.Data;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly LoggerWork loggerWork;

        public LoggerController(ILoggerContext loggerContext)
        {
            loggerWork = new LoggerWork(loggerContext);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogger()
        {
            var result = await loggerWork.GetLoggerAsync();

            return Ok(result);
        }
       
    }
}