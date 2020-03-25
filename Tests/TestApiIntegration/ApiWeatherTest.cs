using Newtonsoft.Json;
using SmartHouse.Domain.Core;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegrationTest
{
    [Collection("Api Weather Test Collection")]
    public class ApiWeatherTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _lient;
        private readonly JsonSerializerSettings _serializerOptions;

        public ApiWeatherTest(TestFixture fixture)
        {
            _lient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
        }

        #region Get weather

        [Fact]
        [Trait("Get Weather", "200")]
        public async Task GetWeather_Success_StatusCode200()
        {
            // Arrange
            const string url = "/api/weather";

            // Act
            var response = await _lient.GetAsync(url);
            var value = await response.Content.ReadAsStringAsync();
            WeatherModel item = JsonConvert.DeserializeObject<WeatherModel>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion Get weather
    }
}