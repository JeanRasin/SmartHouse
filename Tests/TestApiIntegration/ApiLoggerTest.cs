using Newtonsoft.Json;
using SmartHouse.Domain.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegrationTest
{
    [Collection("Api Logger Test Collection")]
    public class ApiLoggerTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _ñlient;
        private readonly JsonSerializerSettings _serializerOptions;

        public ApiLoggerTest(TestFixture fixture)
        {
            _ñlient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
        }

        #region Get logger
        [Fact]
        [Trait("Get Logger", "200")]
        public async Task GetLogger_Success_StatusCode200()
        {
            // Arrange
            const string url = "/api/logger";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();
            List<LoggerModel> items = JsonConvert.DeserializeObject<List<LoggerModel>>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion
    }
}