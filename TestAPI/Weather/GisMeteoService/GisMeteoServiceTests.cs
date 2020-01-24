using NUnit.Framework;
using SmartHouse.Domain.Core;
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
            WeatherModel result = service.GetWeatherAsync().Result;

            Assert.IsNotNull(result);
        }
    }
}