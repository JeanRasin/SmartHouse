using SmartHouse.Domain.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegrationTest
{
    public class ApiLoggerTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _ñlient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiLoggerTest(TestFixture fixture)
        {
            _ñlient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
        }

        [Fact]
        public async Task GetLogger_Success_StatusCode200()
        {
            // Arrange
            var request = "/api/logger";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            List<LoggerModel> items = JsonSerializer.Deserialize<List<LoggerModel>>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}