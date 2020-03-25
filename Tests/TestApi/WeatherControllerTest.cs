using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouseAPI.ApiException;
using SmartHouseAPI.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest
{
    [Collection("Weather controller")]
    public class WeatherControllerTest
    {
        private static readonly WeatherModel _wetaherData = GetTestData();

        private readonly Mock<IWeatherWork> _mockWeatherWork;
        private readonly WeatherController _weatherController;

        public WeatherControllerTest()
        {
            _mockWeatherWork = new Mock<IWeatherWork>();
            _weatherController = new WeatherController(_mockWeatherWork.Object);
        }

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <returns></returns>
        private static WeatherModel GetTestData()
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            WeatherModel result = new Faker<WeatherModel>()
                  .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.WindDeg, f => f.Random.UShort(0, 360))
                  .RuleFor(o => o.Temp, f => f.Random.Float(-100, 100))
                  .RuleFor(o => o.City, f => f.Address.City())
                  .RuleFor(o => o.Pressure, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.Humidity, f => f.Random.Float(0, 1000))
                  .RuleFor(o => o.Description, f => f.Random.Words(5))
                  .RuleFor(o => o.WindSpeed, f => f.Random.Float(0, 1000))
                  .Generate();

            return result;
        }

        #region GetWeatherAsync

        /// <summary>
        /// Get weather.
        /// </summary>
        [Fact]
        [Trait("Get Weather", "Success Status 200")]
        public void GetWeatherAsync_Success_WeatherModelItem()
        {
            // Arrange
            _mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult(_wetaherData));

            // Act
            var result = _weatherController.GetWeatherAsync().Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeatherModel>(result.Value);
            Assert.Equal(result.Value, _wetaherData);
            Assert.Equal(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// No weather data received.
        /// </summary>
        [Fact]
        [Trait("Get Weather", "NotFoundException Status 404")]
        public void GetWeatherAsync_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            _mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult<WeatherModel>(null));

            // Act Assert
            Assert.ThrowsAsync<NotFoundException>(() => _weatherController.GetWeatherAsync());
        }

        /// <summary>
        /// The exception is that the service is not responding.
        /// </summary>
        [Fact]
        [Trait("Get Weather", "Exception Status 500")]
        public void GetWeatherAsync_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockWeatherWork.Setup(m => m.GetWeatherAsync()).Throws<Exception>();

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _weatherController.GetWeatherAsync());
        }

        #endregion GetWeatherAsync
    }
}