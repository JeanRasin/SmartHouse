﻿using Newtonsoft.Json;
using SmartHouse.Domain.Core;
using SmartHouseAPI.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace ApiIntegrationTest
{
    [Collection("Api Goal Test Collection")]
    public class ApiGoalTest : IClassFixture<TestFixture>
    {
        private readonly HttpClient _lient;
        private readonly List<Goal> _itemTestData;
        private readonly JsonSerializerSettings _serializerOptions;

        public ApiGoalTest(TestFixture fixture)
        {
            _lient = fixture.Client;
            _serializerOptions = fixture.SerializerOptions;
            _itemTestData = GetAllGoals().Result;
        }

        /// <summary>
        /// Get a list of goals from the service.
        /// </summary>
        /// <returns></returns>
        private async Task<List<Goal>> GetAllGoals()
        {
            const string url = "/api/goal/getAll";

            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            List<Goal> items = JsonConvert.DeserializeObject<List<Goal>>(value, _serializerOptions);

            if (items.Count() == 0)
            {
                throw new Exception("There are too few items to test.");
            }

            return items;
        }

        #region Get all goals

        [Fact, Order(1)]
        [Trait("Get All Goals", "200")]
        public async Task GetAllGoals_Success_StatusCode200()
        {
            // Arrange
            const string url = "/api/goal/getAll";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();
            List<Goal> items = JsonConvert.DeserializeObject<List<Goal>>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion Get all goals

        #region Get unmarked goals

        [Fact, Order(2)]
        [Trait("Get Unmarked Goals", "200")]
        public async Task GetGoals_Success_StatusCode200()
        {
            // Arrange
            const string url = "/api/goal";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();
            List<Goal> items = JsonConvert.DeserializeObject<List<Goal>>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion Get unmarked goals

        #region Get goal by id

        [Fact, Order(3)]
        [Trait("Get Goal By Id", "200")]
        public async Task GetGoal_Success_StatusCode200()
        {
            // Arrange
            Goal firstItem = _itemTestData.First();
            string url = $"/api/goal/{firstItem.Id}";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();
            Goal item = JsonConvert.DeserializeObject<Goal>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, Order(4)]
        [Trait("Get Goal By Id", "404")]
        public async Task GetGoal_Error_StatusCode404()
        {
            // Arrange
            string url = $"/api/goal/{Guid.NewGuid()}";

            // Act
            HttpResponseMessage response = await _lient.GetAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion Get goal by id

        #region Create goal

        [Fact, Order(8)]
        [Trait("Create Goal", "201")]
        public async Task Create_Success_StatusCode201()
        {
            // Arrange
            const string url = "/api/goal";
            var model = new GoalCreateDto
            {
                Name = "new goal"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PostAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();
            Goal item = JsonConvert.DeserializeObject<Goal>(value, _serializerOptions);

            // Assert
            Assert.NotEmpty(value);
            Assert.NotNull(item);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact, Order(9)]
        [Trait("Create Goal", "400")]
        public async Task Create_Error_StatusCode400()
        {
            // Arrange
            const string url = "/api/goal";
            var model = new GoalCreateDto
            {
                Name = null
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PostAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion Create goal

        #region Update goal

        [Fact, Order(5)]
        [Trait("Update Goal", "204")]
        public async Task Update_Success_StatusCode204()
        {
            // Arrange
            const string url = "/api/goal";
            Goal firstItem = _itemTestData.First();
            var model = new GoalUpdateDto
            {
                Id = firstItem.Id,
                Name = "test goal",
                Done = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PutAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Empty(value);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, Order(6)]
        [Trait("Update Goal", "404")]
        public async Task Update_Error_StatusCode404()
        {
            // Arrange
            const string url = "/api/goal";
            var model = new GoalUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = "test goal",
                Done = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PutAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact, Order(7)]
        [Trait("Update Goal", "400")]
        public async Task Update_Error_StatusCode400()
        {
            // Arrange
            const string url = "/api/goal";
            Goal firstItem = _itemTestData.First();
            var model = new GoalUpdateDto
            {
                Id = firstItem.Id,
                Name = null,
                Done = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PutAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion Update goal

        #region Delete goal

        [Fact, Order(10)]
        [Trait("Delete Goal", "204")]
        public async Task Delete_Success_StatusCode204()
        {
            // Arrange
            Goal firstItem = _itemTestData.Last();
            string url = $"/api/goal/{firstItem.Id}";

            // Act
            HttpResponseMessage response = await _lient.DeleteAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Empty(value);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, Order(11)]
        [Trait("Delete Goal", "404")]
        public async Task Delete_Error_StatusCode404()
        {
            // Arrange
            string url = $"/api/goal/{Guid.NewGuid()}";

            // Act
            HttpResponseMessage response = await _lient.DeleteAsync(url);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotEmpty(value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion Delete goal

        #region Done goal

        [Fact, Order(12)]
        [Trait("Done Goal", "204")]
        public async Task Done_Success_StatusCode204()
        {
            // Arrange
            string url = $"/api/goal/done/";
            Goal firstItem = _itemTestData.First();
            var model = new GoalDoneDto
            {
                Id = firstItem.Id,
                Done = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PutAsync(url, stringContent);
            string value = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Empty(value);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, Order(13)]
        [Trait("Done Goal", "404")]
        public async Task Done_Error_StatusCode404()
        {
            // Arrange
            const string url = "/api/done/";
            var model = new GoalDoneDto
            {
                Id = Guid.NewGuid(),
                Done = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _lient.PutAsync(url, stringContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion Done goal
    }
}