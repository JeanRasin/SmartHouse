using Bogus;
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
    [CollectionDefinition("Weather controller")]
    public class WeatherControllerTest
    {
        private static readonly WeatherModel wetaherData = GetTestData();

        private readonly Mock<IWeatherWork> mockWeatherWork;
        private readonly WeatherController weatherController;

        public WeatherControllerTest()
        {
            mockWeatherWork = new Mock<IWeatherWork>();
            weatherController = new WeatherController(mockWeatherWork.Object);
        }

        static WeatherModel GetTestData()
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
        public void GetWeatherAsync_Success_WeatherModelItem()
        {
            // Arrange
            // var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult(wetaherData));
            // var weatherController = new WeatherController(mockWeatherWork.Object);

            // Act
            var result = weatherController.GetWeatherAsync().Result as OkObjectResult;

            // Assert
            Assert.IsType<WeatherModel>(result.Value);
            Assert.Equal(result.Value, wetaherData);
        }

        /// <summary>
        /// No weather data received.
        /// </summary>
        [Fact]
        public void GetWeatherAsync_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            // var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Returns(Task.FromResult<WeatherModel>(null));
            // var weatherController = new WeatherController(mockWeatherWork.Object);

            // Act Assert
            Assert.ThrowsAsync<NotFoundException>(() => weatherController.GetWeatherAsync());
        }

        /// <summary>
        /// The exception is that the service is not responding.
        /// </summary>
        [Fact]
        public void GetWeatherAsync_Exception_ExceptionStatus500()
        {
            // Arrange
            // var mockWeatherWork = new Mock<IWeatherWork>();
            mockWeatherWork.Setup(m => m.GetWeatherAsync()).Throws<Exception>();
            // var weatherController = new WeatherController(mockWeatherWork.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => weatherController.GetWeatherAsync());
        }
        #endregion
    }
}
