using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.Controllers;
using SmartHouseAPI.Helpers;
using SmartHouseAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTest
{
    public class GoalControllerTest
    {

        public GoalControllerTest()
        {

            //Randomizer.Seed = new Random(1338);

            //List<GoalModel> data = new Faker<GoalModel>()
            //            .StrictMode(true)
            //            .RuleFor(o => o.Id, f => f.Random.Uuid())
            //            .RuleFor(o => o.Name, f => f.Random.Words(3))
            //            .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //            .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //            .RuleFor(o => o.Done, f => f.Random.Bool()).Generate(10);
        }

        [Fact]
        public void GetGoalAll_WhenCalled_ReturnsAllItems()
        {
            Randomizer.Seed = new Random(1338);

            List<GoalModel> goalItems = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate(10);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoalAll()).Returns(() =>
            {
                return goalItems;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoalAll() as OkObjectResult;

            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalItems);
        }

        [Fact]
        public void GetGoalAll_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoalAll()).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoalAll() as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void GetGoals_WhenCalled_ReturnsAllItems()
        {
            Randomizer.Seed = new Random(1338);

            List<GoalModel> goalItems = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate(10);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoals()).Returns(() =>
            {
                return goalItems;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoals() as OkObjectResult;

            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalItems);
        }

        [Fact]
        public void GetGoals_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoals()).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoals() as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void GetGoal_WhenCalled_Return()
        {
            Randomizer.Seed = new Random(1338);

            GoalModel goalItem = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate();

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns(() =>
            {
                return goalItem;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoal(goalItem.Id) as OkObjectResult;

            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalItem);
        }

        [Fact]
        public void GetGoal_WhenCalled_ReturnsStatus404()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns(() =>
            {
                return null;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoal(Guid.NewGuid()) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetGoal_WhenCalled_ReturnsStatus500()
        {
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.GetGoal(Guid.NewGuid()) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Create_WhenCalled_Success()
        {
            Randomizer.Seed = new Random(1338);

            GoalModel goalItem = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate();

            var inputParam = new GoalCreateInput(goalItem.Name);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(() =>
            {
                return goalItem;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Create(inputParam) as OkObjectResult;

            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalItem);
        }

        [Fact]
        public void Create_WhenCalled_ReturnsStatus500()
        {
            Randomizer.Seed = new Random(1338);

            GoalModel goalItem = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate();

            var inputParam = new GoalCreateInput(goalItem.Name);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Create(inputParam) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Create_WhenCalled_ReturnsStatus400()
        {
            Randomizer.Seed = new Random(1338);

            GoalModel goalItem = new Faker<GoalModel>()
                        .StrictMode(true)
                        .RuleFor(o => o.Id, f => f.Random.Uuid())
                        .RuleFor(o => o.Name, f => f.Random.Words(3))
                        .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                        .RuleFor(o => o.Done, f => f.Random.Bool()).Generate();

            var inputParam = new GoalCreateInput();

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(() =>
            {
                return goalItem;
            });

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Create(inputParam) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_WhenCalled_Success()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test name");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Update(inputData) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus400()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Update(inputData) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus404()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test text");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Update(inputData) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Update_WhenCalled_ReturnsStatus500()
        {
            var inputData = new GoalUpdateInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), "test text");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Update(inputData) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Delete_WhenCalled_Success()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Delete(id) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Delete_WhenCalled_ReturnsStatus404()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Delete(id) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Delete_WhenCalled_ReturnsStatus500()
        {
            var id = new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21");
            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();

            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Delete(id) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public void Done_WhenCalled_Success()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>()));

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Done(inputData) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Done_WhenCalled_ReturnsStatus404()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Done(inputData) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Done_WhenCalled_ReturnsStatus500()
        {
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);

            var mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<Exception>();

            var goalController = new GoalController(mockGoalWork.Object);
            var result = goalController.Done(inputData) as StatusCodeResult;

            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
