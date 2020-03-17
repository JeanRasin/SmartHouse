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
        private static readonly List<GoalModel> _goalDataItems = GetTestData();
        private static readonly GoalModel _goalDataItem = _goalDataItems.First();

        private readonly Mock<IGoalWork<GoalModel>> _mockGoalWork;
        private readonly GoalController _goalController;


        public GoalControllerTest()
        {
            _mockGoalWork = new Mock<IGoalWork<GoalModel>>();
            _goalController = new GoalController(_mockGoalWork.Object);
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
            _mockGoalWork.Setup(m => m.GetGoalAll()).Returns(_goalDataItems);

            // Act
            var result = _goalController.GetGoalAll() as OkObjectResult;

            // Assert
            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, _goalDataItems);
        }

        /// <summary>
        ///  An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoalAll_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockGoalWork.Setup(m => m.GetGoalAll()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.GetGoalAll());
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
            _mockGoalWork.Setup(m => m.GetGoals()).Returns(_goalDataItems);

            // Act
            var result = _goalController.GetGoals() as OkObjectResult;

            //Assert
            Assert.IsType<List<GoalModel>>(result.Value);
            Assert.Equal(result.Value, _goalDataItems);
        }

        /// <summary>
        /// An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoals_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockGoalWork.Setup(m => m.GetGoals()).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.GetGoals());
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
            _mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns(_goalDataItem);

            // Act
            var result = _goalController.GetGoal(_goalDataItem.Id) as OkObjectResult;

            //Assert
            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, _goalDataItem);
        }

        /// <summary>
        /// Goal by id not found.
        /// </summary>
        [Fact]
        public void GetGoal_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            _mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _goalController.GetGoal(Guid.NewGuid()));
        }

        /// <summary>
        /// An exception has occurred.
        /// </summary>
        [Fact]
        public void GetGoal_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockGoalWork.Setup(m => m.GetGoal(It.IsAny<Guid>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.GetGoal(Guid.NewGuid()));
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
            var inputParam = new GoalCreateDto(_goalDataItem.Name);
            _mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(_goalDataItem);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns($"http://localhost:5555/api/goal/{_goalDataItem.Id}");

            var goalController = new GoalController(_mockGoalWork.Object)
            {
                Url = mockUrlHelper.Object
            };

            // Act
            var result = goalController.Create(inputParam) as CreatedResult;

            // Assert
            Assert.IsType<GoalModel>(result.Value);
            Assert.Equal(result.Value, _goalDataItem);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }

        /// <summary>
        /// Goal creation exception.
        /// </summary>
        [Fact]
        public void Create_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputParam = new GoalCreateDto(_goalDataItem.Name);
            _mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.Create(inputParam));
        }

        /// <summary>
        /// ModelState exception.
        /// </summary>
        [Fact]
        public void Create_Exception_ModelStateExceptionStatus400()
        {
            // Arrange
            var inputParam = new GoalCreateDto();
            _mockGoalWork.Setup(m => m.Create(It.IsAny<string>())).Returns(_goalDataItem);
            _goalController.ModelState.AddModelError("key", "error message");

            // Act & Assert
            Assert.Throws<ModelStateException>(() => _goalController.Create(inputParam));
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
            var inputData = new GoalUpdateDto(Guid.NewGuid(), "test name");
            _mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));

            // Act
            var result = _goalController.Update(inputData) as NoContentResult;

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
            var inputData = new GoalUpdateDto(Guid.NewGuid(), "");
            _mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>()));
            _goalController.ModelState.AddModelError("key", "error message");

            // Act & Assert
            Assert.Throws<ModelStateException>(() => _goalController.Update(inputData));
        }

        /// <summary>
        /// Update goal by id not found.
        /// </summary>
        [Fact]
        public void Update_IdNotFound_NotFoundExceptionStatus404()
        {
            // Arrange
            var inputData = new GoalUpdateDto(Guid.NewGuid(), "test text");
            _mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _goalController.Update(inputData));
        }

        /// <summary>
        /// Goal creation exception.
        /// </summary>
        [Fact]
        public void Update_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputData = new GoalUpdateDto(Guid.NewGuid(), "test text");
            _mockGoalWork.Setup(m => m.Update(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.Update(inputData));
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
            _mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>()));

            // Act 
            var result = _goalController.Delete(Guid.NewGuid()) as NoContentResult;

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
            _mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _goalController.Delete(Guid.NewGuid()));
        }

        /// <summary>
        /// Goal delete exception.
        /// </summary>
        [Fact]
        public void Delete_Exception_ExceptionStatus500()
        {
            // Arrange
            _mockGoalWork.Setup(m => m.Delete(It.IsAny<Guid>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.Delete(Guid.NewGuid()));
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
            var inputData = new GoalDoneDto(Guid.NewGuid(), true);
            _mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>()));

            // Act
            var result = _goalController.Done(inputData) as NoContentResult;

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
            var inputData = new GoalDoneDto(Guid.NewGuid(), true);
            _mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<KeyNotFoundException>();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _goalController.Done(inputData));
        }
        /// <summary>
        /// Goal done exception.
        /// </summary>
        [Fact]
        public void Done_Exception_ExceptionStatus500()
        {
            // Arrange
            var inputData = new GoalDoneDto(new Guid("7dc7631d-3f1e-f8bb-166c-63b52a05db21"), true);
            _mockGoalWork.Setup(m => m.Done(It.IsAny<Guid>(), It.IsAny<bool>())).Throws<Exception>();

            // Act & Assert
            Assert.Throws<Exception>(() => _goalController.Done(inputData));
        }
        #endregion
    }
}
