using NUnit.Framework;
using SmartHouse.Domain.Core;
using SmartHouse.Service.Weather.Gismeteo;
using System.Threading.Tasks;

namespace TestService
{
    public class GisMeteoServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Get_Success_WeatherModelItem()
        {
            // Arrange
            var service = new GisMeteoService();

            // Act
            WeatherModel result = await service.GetWeatherAsync();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}