using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartHouse.Domain.Core;
using SmartHouseAPI;
using SmartHouseAPI.InputModel;
using Xunit;

namespace ApiIntegrationTest
{
    public class ApiWeatherTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _�lient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiWeatherTest(TestFixture fixture)
        {
            _�lient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
        }

        [Fact]
        public async Task GetWeather_Success_StatusCode200()
        {
            // Arrange
            var request = "/api/weather";

            // Act
            var response = await _�lient.GetAsync(request);
            var value = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
           
            Assert.True(false);
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