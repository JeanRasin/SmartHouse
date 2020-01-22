using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data.Weather;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Domain.Interfaces.Weather;
using System.Threading.Tasks;

namespace SmartHouseAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherWork weatherWork;

        public WeatherController(IWeatherService ws)
        {
            weatherWork = new WeatherWork(ws);
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "WeatherCaching")]
        public async Task<IActionResult> GetWeatherAsync()
        {
            var result = await weatherWork.GetWeatherAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        /*
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}