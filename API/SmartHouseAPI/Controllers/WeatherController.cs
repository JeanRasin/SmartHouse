using Microsoft.AspNetCore.Mvc;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Interfaces;
using System;
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

        // GET api/weather
        [HttpGet]
        [Produces("application/json")]
        [ResponseCache(CacheProfileName = "WeatherCaching")]
        public async Task<IActionResult> GetWeatherAsync()
        {
            try
            {
                var result = await weatherWork.GetWeatherAsync();

                if (result == null)
                {
                    throw new Exception("Not data.");
                }

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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