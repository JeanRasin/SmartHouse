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
        public async Task Get_Success_Data()
        {
            var service = new GisMeteoService();
            WeatherModel result = await service.GetWeatherAsync();

            Assert.IsNotNull(result);
        }
    }
}