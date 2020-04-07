using Bogus;
using Moq;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BusinessTest
{
    [Collection("Goal work")]
    public class GoalWorkTest
    {
        private static readonly List<Goal> _testDataItems = GetTestData();
        private static readonly Goal _testDataItem = _testDataItems.First();

        private readonly Mock<IGoalRepository<Goal>> _mockGoalRepository;
        private readonly GoalWork _goalWork;

        public GoalWorkTest()
        {
            _mockGoalRepository = new Mock<IGoalRepository<Goal>>();
            _goalWork = new GoalWork(_mockGoalRepository.Object);
        }

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static List<Goal> GetTestData(int n = 10)
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            List<Goal> result = new Faker<Goal>()
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
        /// Get all the goals.
        /// </summary>
        [Fact]
        [Trait("Get All Goals", "Success")]
        public void GetGoalAll_OrderByDescendingDateUpdate_GoalModelItems()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoals()).Returns(_testDataItems);

            // Act
            IEnumerable<Goal> result = _goalWork.GetGoalAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), _testDataItems.Count());
        }

        #endregion GetGoalAll

        #region GetGoals

        /// <summary>
        /// Get unmarked goals.
        /// </summary>
        [Fact]
        [Trait("Get Unmarked Goals", "Success")]
        public void GetGoals_DoneIsFalse_GoalModelItems()
        {
            //Arrange
            Randomizer.Seed = new Random(1338);

            List<Goal> testData = new Faker<Goal>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid())
                 .RuleFor(o => o.Name, f => f.Random.Words(3))
                 .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.Done, f => f.Random.Bool())
                 .Generate(10);

            // Exactly what would be true.
            testData[0].Done = true;

            _mockGoalRepository.Setup(s => s.GetGoals()).Returns(testData);

            // Act
            IEnumerable<Goal> result = _goalWork.GetGoals();

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Count(), testData.Count());
            Assert.True(result.All(a => a.Done == false));
        }

        #endregion GetGoals

        #region GetGoal

        /// <summary>
        /// Get goal by id.
        /// </summary>
        [Fact]
        [Trait("Get Goal By Id", "Success")]
        public void GetGoal_Success_GoalModelItem()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(_testDataItem);

            // Act
            Goal result = _goalWork.GetGoal(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, _testDataItem);
        }

        /// <summary>
        /// Get goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        [Trait("Get Goal By Id", "KeyNotFoundException")]
        public void GetGoal_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((Goal)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _goalWork.GetGoal(Guid.NewGuid()));
        }

        #endregion GetGoal

        #region Create

        /// <summary>
        /// Create goal.
        /// </summary>
        [Fact]
        [Trait("Create Goal", "Success")]
        public void Create_Success_GoalModelItem()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.Create(It.IsAny<Goal>())).Verifiable();
            _mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            Goal result = _goalWork.Create("test name");

            //Assert
            Assert.NotNull(result);
            _mockGoalRepository.Verify(v => v.Create(It.IsAny<Goal>()), Times.Once);
            _mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        #endregion Create

        #region Update

        /// <summary>
        /// Update goal.
        /// </summary>
        [Fact]
        [Trait("Update Goal", "Success")]
        public void Update_Success()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(_testDataItem);
            _mockGoalRepository.Setup(s => s.Update(It.IsAny<Goal>())).Verifiable();
            _mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            _goalWork.Update(Guid.NewGuid(), "test name", true);

            //Assert
            _mockGoalRepository.Verify(v => v.Update(It.IsAny<Goal>()), Times.Once);
            _mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Update goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        [Trait("Update Goal", "KeyNotFoundException")]
        public void Update_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((Goal)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _goalWork.Update(Guid.NewGuid(), "test name", true));
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// Delete goal by id.
        /// </summary>
        [Fact]
        [Trait("Delete Goal", "Success")]
        public void Delete_Success()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(_testDataItem);
            _mockGoalRepository.Setup(s => s.Remove(It.IsAny<Guid>())).Verifiable();
            _mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            _goalWork.Delete(Guid.NewGuid());

            //Assert
            _mockGoalRepository.Verify(v => v.Remove(It.IsAny<Guid>()), Times.Once);
            _mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Delete goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        [Trait("Delete Goal", "KeyNotFoundException")]
        public void Delete_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((Goal)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _goalWork.Delete(Guid.NewGuid()));
        }

        #endregion Delete

        #region Done

        /// <summary>
        /// Mark goal.
        /// </summary>
        [Fact]
        [Trait("Done Goal", "Success")]
        public void Done_Success()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoals()).Returns(_testDataItems);
            _mockGoalRepository.Setup(s => s.Update(It.IsAny<Goal>())).Verifiable();
            _mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            _goalWork.Done(_testDataItem.Id, !_testDataItem.Done);

            //Assert
            _mockGoalRepository.Verify(v => v.Update(It.IsAny<Goal>()), Times.Once);
            _mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Mark goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        [Trait("Done Goal", "KeyNotFoundException")]
        public void Done_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            _mockGoalRepository.Setup(s => s.GetGoals()).Returns(_testDataItems);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _goalWork.Done(Guid.NewGuid(), true));
        }

        #endregion Done
    }
}