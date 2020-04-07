using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegrationTest
{
    [Collection("Api Doc Test Collection")]
    public class ApiDocTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _lient;

        public ApiDocTest(TestFixture fixture)
        {
            _lient = fixture.Client;
        }

        #region Get swagger

        [Fact]
        [Trait("Get Swagger JSON", "200")]
        public async Task GetSwagger_Success_Json()
        {
            // Arrange
            const string url = "/api/docs/v1/swagger.json";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Get Swagger HTML", "200")]
        public async Task GetSwagger_Success_Index()
        {
            // Arrange
            const string url = "/api/docs/index.html";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion Get swagger
    }
}