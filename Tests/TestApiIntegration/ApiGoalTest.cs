using SmartHouse.Domain.Core;
using SmartHouseAPI.InputModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegrationTest
{
    public class ApiGoalTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _ñlient;
        private readonly GoalModel _itemTestData;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiGoalTest(TestFixture fixture)
        {
            _ñlient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
        }

        [Fact]
        public async Task GetSwagger_Success_Json()
        {
            // Arrange
            var request = "/api/docs/v1/swagger.json";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSwagger_Success_Index()
        {
            // Arrange
            var request = "/api/docs/index.html";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllGoals_Success_StatusCode200()
        {
            // Arrange
            var request = "/api/goal/getAll";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            List<GoalModel> items = JsonSerializer.Deserialize<List<GoalModel>>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetGoals_Success_StatusCode200()
        {
            // Arrange
            var request = "/api/goal";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            List<GoalModel> items = JsonSerializer.Deserialize<List<GoalModel>>(value);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetGoal_Success_StatusCode200()
        {
            // Arrange
            var request = $"/api/goal/333"; //todo:!!!

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();
            GoalModel item = JsonSerializer.Deserialize<GoalModel>(value);

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGoal_Error_StatusCode404()
        {
            // Arrange
            var request = $"/api/goal/{Guid.NewGuid()}";

            // Act
            HttpResponseMessage response = await _ñlient.GetAsync(request);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_Success_StatusCode201()
        {
            // Arrange
            var request = $"/api/goal";
            var model = new GoalCreateDto
            {
                Name = "new goal"
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PostAsync(request, stringContent);
            string value = await response.Content.ReadAsStringAsync();
            GoalModel item = JsonSerializer.Deserialize<GoalModel>(value, _serializerOptions);//todo: exception deserialize model

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Create_Error_StatusCode400()
        {
            // Arrange
            var request = $"/api/goal";
            var model = new GoalCreateDto
            {
                Name = null
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PostAsync(request, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Upddate_Success_StatusCode204()
        {
            // Arrange
            var request = $"/api/goal";
            var model = new GoalUpdateDto
            {
                Id = Guid.NewGuid(),//todo:ïðàâèëüíûé id
                Name = "test goal",
                Done = true
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PutAsync(request, stringContent);
            string value = await response.Content.ReadAsStringAsync();
           // GoalModel item = JsonSerializer.Deserialize<GoalModel>(value, _serializerOptions);//todo: exception deserialize model

            // Assert
            Assert.NotEmpty(value);
           // Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Upddate_Error_StatusCode404()
        {
            // Arrange
            var request = $"/api/goal";
            var model = new GoalUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = "test goal",
                Done = true
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PutAsync(request, stringContent);
           // string value = await response.Content.ReadAsStringAsync();

            // Assert
           // Assert.Empty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Upddate_Error_StatusCode400()
        {
            // Arrange
            var request = $"/api/goal";
            var model = new GoalUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = null,
                Done = true
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PutAsync(request, stringContent);
            //string value = await response.Content.ReadAsStringAsync();

            // Assert
            //Assert.Empty(value);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Success_StatusCode204()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var request = $"/api/goal/{id}"; //todo:!id
           
            // Act
            HttpResponseMessage response = await _ñlient.DeleteAsync(request);
            //string value = await response.Content.ReadAsStringAsync();
            // GoalModel item = JsonSerializer.Deserialize<GoalModel>(value, _serializerOptions);//todo: exception deserialize model

            // Assert
            //Assert.NotEmpty(value);
            // Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Error_StatusCode404()
        {
            // Arrange
            var request = $"/api/goal/{Guid.NewGuid()}";

            // Act
            HttpResponseMessage response = await _ñlient.DeleteAsync(request);
            //string value = await response.Content.ReadAsStringAsync();
            // GoalModel item = JsonSerializer.Deserialize<GoalModel>(value, _serializerOptions);//todo: exception deserialize model

            // Assert
            //Assert.NotEmpty(value);
            // Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Done_Success_StatusCode204()
        {
            // Arrange
            var request = $"/api/goal/done/";
            var model = new GoalDoneDto
            {
                Id = Guid.NewGuid(),//todo:id
                Done = true
            };
            var stringContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _ñlient.PutAsync(request, stringContent);
            //string value = await response.Content.ReadAsStringAsync();
            // GoalModel item = JsonSerializer.Deserialize<GoalModel>(value, _serializerOptions);//todo: exception deserialize model

            // Assert
            //Assert.NotEmpty(value);
            // Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        /*
        [Fact]
        public async Task TestGetStockItemAsync()
        {
            // Arrange
            var request = "/api/v1/Warehouse/StockItem/1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPostStockItemAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/Warehouse/StockItem",
                Body = new
                {
                    StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
                    SupplierID = 12,
                    UnitPackageID = 7,
                    OuterPackageID = 7,
                    LeadTimeDays = 14,
                    QuantityPerOuter = 1,
                    IsChillerStock = false,
                    TaxRate = 15.000m,
                    UnitPrice = 32.00m,
                    RecommendedRetailPrice = 47.84m,
                    TypicalWeightPerUnit = 0.050m,
                    CustomFields = "{ \"CountryOfManufacture\": \"Japan\", \"Tags\": [\"32GB\",\"USB Powered\"] }",
                    Tags = "[\"32GB\",\"USB Powered\"]",
                    SearchDetails = "USB anime flash drive - Vegeta",
                    LastEditedBy = 1,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddYears(5)
                }
            };

            // Act
            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPutStockItemAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v1/Warehouse/StockItem/1",
                Body = new
                {
                    StockItemName = string.Format("USB anime flash drive - Vegeta {0}", Guid.NewGuid()),
                    SupplierID = 12,
                    Color = 3,
                    UnitPrice = 39.00m
                }
            };

            // Act
            var response = await Client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestDeleteStockItemAsync()
        {
            // Arrange

            var postRequest = new
            {
                Url = "/api/v1/Warehouse/StockItem",
                Body = new
                {
                    StockItemName = string.Format("Product to delete {0}", Guid.NewGuid()),
                    SupplierID = 12,
                    UnitPackageID = 7,
                    OuterPackageID = 7,
                    LeadTimeDays = 14,
                    QuantityPerOuter = 1,
                    IsChillerStock = false,
                    TaxRate = 10.000m,
                    UnitPrice = 10.00m,
                    RecommendedRetailPrice = 47.84m,
                    TypicalWeightPerUnit = 0.050m,
                    CustomFields = "{ \"CountryOfManufacture\": \"USA\", \"Tags\": [\"Sample\"] }",
                    Tags = "[\"Sample\"]",
                    SearchDetails = "Product to delete",
                    LastEditedBy = 1,
                    ValidFrom = DateTime.Now,
                    ValidTo = DateTime.Now.AddYears(5)
                }
            };

            // Act
            var postResponse = await Client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();

            var singleResponse = JsonConvert.DeserializeObject<SingleResponse<StockItem>>(jsonFromPostResponse);

            var deleteResponse = await Client.DeleteAsync(string.Format("/api/v1/Warehouse/StockItem/{0}", singleResponse.Model.StockItemID));

            // Assert
            postResponse.EnsureSuccessStatusCode();

            Assert.False(singleResponse.DidError);

            deleteResponse.EnsureSuccessStatusCode();
        }
        */
    }
}