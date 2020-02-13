using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace ApiTest
{
    public class WeatherControllerTest
    {
        readonly WeatherModel wetaherData;
        readonly ILogger log;

        public WeatherControllerTest()
        {
            Randomizer.Seed = new Random(1338);

            wetaherData = new Faker<WeatherModel>()
                  .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.WindDeg, f => f.Random.UShort(0, 360))
                  .RuleFor(o => o.Temp, f => f.Random.Float(-100, 100))
                  .RuleFor(o => o.City, f => f.Address.City())
                  .RuleFor(o => o.Pressure, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.Humidity, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.Description, f => f.Random.Words(5))
                  .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                  .Generate();

            log = Mock.Of<ILogger<WeatherController>>();
        }

        #region GetWeatherAsync
        [Fact]
        public async void GetWeatherAsync_WhenCalled_ReturnsResult()
        {
            var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult(wetaherData));

            var weatherController = new WeatherController(mockWeatherWork.Object, log);
            var result = await weatherController.GetWeatherAsync();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetWeatherAsync_WhenCalled_ReturnsAllItems()
        {
            var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult(wetaherData));

            var weatherController = new WeatherController(mockWeatherWork.Object, log);
            var result = weatherController.GetWeatherAsync().Result as OkObjectResult;

            Assert.IsType<WeatherModel>(result.Value);
            Assert.Equal(result.Value, wetaherData);
        }

        [Fact]
        public void GetWeatherAsync_WhenCalled_ReturnsStatus404()
        {
            var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult<WeatherModel>(null));

            var weatherController = new WeatherController(mockWeatherWork.Object, log);
            var result = weatherController.GetWeatherAsync().Result as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public void GetWeatherAsync_WhenCalled_ReturnsStatus500()
        {
            var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Throws<Exception>();

            var weatherController = new WeatherController(mockWeatherWork.Object, log);
            var result = weatherController.GetWeatherAsync().Result as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
        #endregion
    }
}
