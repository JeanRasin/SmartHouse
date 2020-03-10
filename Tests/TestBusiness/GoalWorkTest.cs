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
    [CollectionDefinition("Goal work")]
    public class GoalWorkTest
    {
        private static List<GoalModel> testDataItems;
        private static GoalModel testDataItem;

        private readonly Mock<IGoalRepository<GoalModel>> mockGoalRepository;
        private readonly GoalWork goalWork;

        public GoalWorkTest()
        {
            GetTestData();

            mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();

            goalWork = new GoalWork(mockGoalRepository.Object);
        }

        static void GetTestData()
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            testDataItems = new Faker<GoalModel>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid())
                 .RuleFor(o => o.Name, f => f.Random.Words(3))
                 .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.Done, f => f.Random.Bool())
                 .Generate(10);

            testDataItem = new Faker<GoalModel>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid())
                 .RuleFor(o => o.Name, f => f.Random.Words(3))
                 .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.Done, f => f.Random.Bool())
                 .Generate();
        }

        #region GetGoalAll
        /// <summary>
        /// Get all the goals.
        /// </summary>
        [Fact]
        public void GetGoalAll_OrderByDescendingDateUpdate_GoalModelItems()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);

            // Act
            IEnumerable<GoalModel> result = goalWork.GetGoalAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), testDataItems.Count());

        }
        #endregion

        #region GetGoals
        /// <summary>
        /// Get outstanding goals.
        /// </summary>
        [Fact]
        public void GetGoals_DoneIsFalse_GoalModelItems()
        {
            //Arrange
            Randomizer.Seed = new Random(1338);

            List<GoalModel> testData = new Faker<GoalModel>()
                 .StrictMode(true)
                 .RuleFor(o => o.Id, f => f.Random.Uuid())
                 .RuleFor(o => o.Name, f => f.Random.Words(3))
                 .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                 .RuleFor(o => o.Done, f => f.Random.Bool())
                 .Generate(10);

            // Exactly what would be true.
            testData[0].Done = true;

            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testData);

            // Act
            IEnumerable<GoalModel> result = goalWork.GetGoals();

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Count(), testData.Count());
            Assert.True(result.All(a => a.Done == false));
        }
        #endregion

        #region GetGoal
        /// <summary>
        /// Get goal by id.
        /// </summary>
        [Fact]
        public void GetGoal_Success_GoalModelItem()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);

            // Act
            GoalModel result = goalWork.GetGoal(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, testDataItem);
        }

        /// <summary>
        /// Get goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void GetGoal_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create
        /// <summary>
        /// Create goal.
        /// </summary>
        [Fact]
        public void Create_Success_GoalModelItem()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.Create(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            GoalModel result = goalWork.Create("test name");

            //Assert
            Assert.NotNull(result);
            mockGoalRepository.Verify(v => v.Create(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update goal.
        /// </summary>
        [Fact]
        public void Update_Success()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);
            mockGoalRepository.Setup(s => s.Update(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            goalWork.Update(Guid.NewGuid(), "test name", true);

            //Assert
            mockGoalRepository.Verify(v => v.Update(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Update goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void Update_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Update(Guid.NewGuid(), "test name", true));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete goal by id.
        /// </summary>
        [Fact]
        public void Delete_Success()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);
            mockGoalRepository.Setup(s => s.Remove(It.IsAny<Guid>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            goalWork.Delete(Guid.NewGuid());

            //Assert
            mockGoalRepository.Verify(v => v.Remove(It.IsAny<Guid>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Delete goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void Delete_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Delete(Guid.NewGuid()));
        }
        #endregion

        #region Done
        /// <summary>
        /// Mark goal.
        /// </summary>
        [Fact]
        public void Done_Success()
        {
            //Arrange
            GoalModel testDataItem = testDataItems.First();

            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);
            mockGoalRepository.Setup(s => s.Update(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            // Act
            goalWork.Done(testDataItem.Id, !testDataItem.Done);

            //Assert
            mockGoalRepository.Verify(v => v.Update(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        /// <summary>
        /// Mark goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void Done_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Done(Guid.NewGuid(), true));
        }
        #endregion
    }
}
