using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using SmartHouseAPI.ApiException;
using SmartHouseAPI.Controllers;
using SmartHouseAPI.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTest
{
    [CollectionDefinition("Goal controller")]
    public class GoalControllerTest
    {
        private static readonly List<GoalModel> goalDataItems = GetTestData();
        private static readonly GoalModel goalDataItem = goalDataItems.First();

        private readonly Mock<IGoalWork<GoalModel>> mockGoalWork;
        private readonly GoalController goalController;


        public GoalControllerTest()
        {
            mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            goalController = new GoalController(mockGoalWork.Object);
        }

        static List<GoalModel> GetTestData(int n = 10)
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            List<GoalModel> result = new Faker<GoalModel>()
                       .StrictMode(true)
                       .RuleFor(o => o.Id, f => f.Random.Uuid())
                       .RuleFor(o => o.Name, f => f.Random.Words(3))
                       .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                       .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                       .RuleFor(o => o.Done, f => f.Random.Bool())
                       .Generate(n);

            return result;
        }

        #region GetGoalAll
        /// <summary>
        /// Get all goals.
        /// </summary>
        [Fact]
        public void GetGoalAll_All_GoalModelItems()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoalAll()).Returns(goalDataItems);

            // Act
            var result = goalController.GetGoalAll() as OkObjectResult;

            // Assert
            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalDataItems);
        }

        /// <summary>
        ///  An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoalAll_Exception_ExceptionStatus500()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoalAll()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.GetGoalAll());
        }
        #endregion

        #region GetGoals
        /// <summary>
        /// Get goals.
        /// </summary>
        [Fact]
        public void GetGoals_GoalModelItems()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoals()).Returns(goalDataItems);

            // Act
            var result = goalController.GetGoals() as OkObjectResult;

            //Assert
            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, goalDataItems);
        }

        /// <summary>
        /// An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoals_Exception_ExceptionStatus500()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoals()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.GetGoals());
        }
        #endregion

        #region GetGoal
        /// <summary>
        /// Get goal by id.
        /// </summary>
        [Fact]
        public void GetGoal_GoalModelItem()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns(goalDataItem);

            // Act
            var result = goalController.GetGoal(goalDataItem.Id) as OkObjectResult;

            //Assert
            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalDataItem);
        }

        /// <summary>
        /// Goal by id not found.
        /// </summary>
        [Fact]
        public void GetGoal_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => goalController.GetGoal(Guid.NewGuid()));
        }

        /// <summary>
        /// An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoal_Exception_ExceptionStatus500()
        {
            // Arrange
            mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create
        /// <summary>
        /// Create goal.
        /// </summary>
        [Fact]
        public void Create_Success_Status201()
        {
            // Arrange
            var inputParam = new GoalCreateInput(goalDataItem.Name);
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(goalDataItem);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns($"http://localhost:5555/api/goal/{goalDataItem.Id}");

            var goalController = new GoalController(mockGoalWork.Object)
            {
                Url = mockUrlHelper.Object
            };

            // Act
            var result = goalController.Create(inputParam) as CreatedResult;

            // Assert
            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, goalDataItem);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        /// <summary>
        /// Goal creation exception.
        /// </summary>
        [Fact]
        public void Create_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputParam = new GoalCreateInput(goalDataItem.Name);
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.Create(inputParam));
        }

        /// <summary>
        /// ModelState exception.
        /// </summary>
        [Fact]
        public void Create_Exception_ModelStateExceptionStatus400()
        {
            // Arrange
            var inputParam = new GoalCreateInput();
            mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(goalDataItem);
            goalController.ModelState.AddModelError("key", "error message");

            // Act & Assert
            Assert.Throws<ModelStateException>(() => goalController.Create(inputParam));
        }
        #endregion

        #region Update
        /// <summary>
        /// Goal update.
        /// </summary>
        [Fact]
        public void Update_Success_Status204()
        {
            // Arrange
            var inputData = new GoalUpdateInput(Guid.NewGuid(), "test name");
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));

            // Act
            var result = goalController.Update(inputData) as NoContentResult;

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        /// <summary>
        /// ModelState exception.
        /// </summary>
        [Fact]
        public void Update_Exception_ModelStateExceptionStatus400()
        {
            // Arrange
            var inputData = new GoalUpdateInput(Guid.NewGuid(), "");
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));
            goalController.ModelState.AddModelError("key", "error message");

            // Act & Assert
            Assert.Throws<ModelStateException>(() => goalController.Update(inputData));
        }

        /// <summary>
        /// Update goal by id not found.
        /// </summary>
        [Fact]
        public void Update_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            var inputData = new GoalUpdateInput(Guid.NewGuid(), "test text");
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => goalController.Update(inputData));
        }

        /// <summary>
        /// Goal creation exception.
        /// </summary>
        [Fact]
        public void Update_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputData = new GoalUpdateInput(Guid.NewGuid(), "test text");
            mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.Update(inputData));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete goal by id.
        /// </summary>
        [Fact]
        public void Delete_Success_Status204()
        {
            // Arrange
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>()));

            // Act 
            var result = goalController.Delete(Guid.NewGuid()) as NoContentResult;

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        /// <summary>
        /// Delete goal by id not found.
        /// </summary>
        [Fact]
        public void Delete_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => goalController.Delete(Guid.NewGuid()));
        }

        /// <summary>
        /// Goal delete exception.
        /// </summary>
        [Fact]
        public void Delete_Exception_ExceptionStatus500()
        {
            // Arrange
            mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.Delete(Guid.NewGuid()));
        }
        #endregion

        #region Done
        /// <summary>
        /// Mark goal.
        /// </summary>
        [Fact]
        public void Done_Success_Status204()
        {
            // Arrange
            var inputData = new GoalDoneInput(Guid.NewGuid(), true);
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>()));

            // Act
            var result = goalController.Done(inputData) as NoContentResult;

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }

        /// <summary>
        /// Done goal by id not found.
        /// </summary>
        [Fact]
        public void Done_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            var inputData = new GoalDoneInput(Guid.NewGuid(), true);
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => goalController.Done(inputData));
        }
        /// <summary>
        /// Goal done exception.
        /// </summary>
        [Fact]
        public void Done_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputData = new GoalDoneInput(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);
            mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => goalController.Done(inputData));
        }
        #endregion
    }
}
