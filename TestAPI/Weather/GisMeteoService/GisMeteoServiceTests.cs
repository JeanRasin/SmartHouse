using NUnit.Framework;
using SmartHouse.Domain.Core.Weather;
using SmartHouse.Service.Weather.Gismeteo;

namespace TestAPI
{
    public class GisMeteoServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_Success_Data()
        {
            var service = new GisMeteoService();
            WeatherData result = service.GetWeatherAsync().Result;

            Assert.IsNotNull(result);
        }
    }
}