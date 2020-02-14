﻿using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.Controllers;
using SmartHouseAPI.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTest
{
    public class GoalControllerTest
    {
        readonly GoalModel goalDataItem;
        readonly List<GoalModel> goalDataItems;

        public GoalControllerTest()
        {
            Randomizer.Seed = new Random(1338);

            goalDataItem = new Faker<GoalModel>()
                       .StrictMode(true)
                       .RuleFor(o => o.Id, f => f.Random.Uuid())
                       .RuleFor(o => o.Name, f => f.Random.Words(3))
                       .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                       .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                       .RuleFor(o => o.Done, f => f.Random.Bool()).Generate();

            goalDataItems = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate(10);
        }

        #region GetGoalAll
        [Fact]
        public void GetGoalAll_WhenCalled_ReturnsAllItems()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoalAll()).Returns(() =>
            {
                return goalDataItems;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoalAll() as OkObjectResult;

            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalDataItems);
        }

        [Fact]
        public void GetGoalAll_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoalAll()).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.GetGoalAll());
        }
        #endregion

        #region GetGoals
        [Fact]
        public void GetGoals_WhenCalled_ReturnsAllItems()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoals()).Returns(() =>
            {
                return goalDataItems;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoals() as OkObjectResult;

            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalDataItems);
        }

        [Fact]
        public void GetGoals_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoals()).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.GetGoals());
        }
        #endregion

        #region GetGoal
        [Fact]
        public void GetGoal_WhenCalled_Return()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns(goalDataItem);

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoal(goalDataItem.Id) as OkObjectResult;

            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalDataItem);
        }

        [Fact]
        public void GetGoal_WhenCalled_ReturnsStatus404()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<NotFoundException>(() => goalController.GetGoal(Guid.NewGuid()));
        }

        [Fact]
        public void GetGoal_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create
        [Fact]
        public void Create_WhenCalled_Success201()
        {
            var inputParam = new GoalCreateInput(goalDataItem.Name);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(() =>
            {
                return goalDataItem;
            });

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns($"http://localhost:5555/api/goal/{goalDataItem.Id}");

            var goalController = new GoalController(mockGoalWork.Object)
            {
                Url = mockUrlHelper.Object
            };

            var result = goalController.Create(inputParam) as CreatedResult;

            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalDataItem);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        [Fact]
        public void Create_WhenCalled_ReturnsStatus500()
        {
            var inputParam = new GoalCreateInput(goalDataItem.Name);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.Create(inputParam));
        }

        [Fact]
        public void Create_WhenCalled_ReturnsStatus400()
        {
            var inputParam = new GoalCreateInput();

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(() =>
            {
                return goalDataItem;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            goalController.ModelState.AddModelError("key", "error message");

            Assert.Throws<ModelStateException>(() => goalController.Create(inputParam));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_WhenCalled_Success()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test name");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Update(inputData) as NoContentResult;

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus400()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>()));

            var goalController = new GoalController(mockGoalWork.Object);
            goalController.ModelState.AddModelError("key", "error message");

            Assert.Throws<ModelStateException>(() => goalController.Update(inputData));
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus404()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test text");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<NotFoundException>(() => goalController.Update(inputData));
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus500()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test text");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.Update(inputData));
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_WhenCalled_Success()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Delete(id) as NoContentResult;

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        [Fact]
        public void Delete_WhenCalled_ReturnsStatus404()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<NotFoundException>(() => goalController.Delete(id));
        }

        [Fact]
        public void Delete_WhenCalled_ReturnsStatus500()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.Delete(id));
        }

        [Fact]
        public void Done_WhenCalled_Success()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Done(inputData) as NoContentResult;

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }
        #endregion

        #region Done
        [Fact]
        public void Done_WhenCalled_ReturnsStatus404()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<NotFoundException>(() => goalController.Done(inputData));
        }

        [Fact]
        public void Done_WhenCalled_ReturnsStatus500()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);

            Assert.Throws<Exception>(() => goalController.Done(inputData));
        }
        #endregion
    }
}
