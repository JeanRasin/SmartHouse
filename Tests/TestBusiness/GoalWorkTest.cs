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
        private readonly List<GoalModel> testDataItems;
        private readonly GoalModel testDataItem;

        public GoalWorkTest()
        {
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
        [Fact]
        public void GetGoalAll_OrderByDescendingDateUpdate_GoalModelItems()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);
            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            IEnumerable<GoalModel> result = goalWork.GetGoalAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), testDataItems.Count());

        }
        #endregion

        #region GetGoals
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

            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testData);
            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            IEnumerable<GoalModel> result = goalWork.GetGoals();

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Count(), testData.Count());
            Assert.True(result.All(a => a.Done == false));
        }
        #endregion

        #region GetGoal
        [Fact]
        public void GetGoal_Success_GoalModelItem()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);
            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            GoalModel result = goalWork.GetGoal(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, testDataItem);
        }

        [Fact]
        public void GetGoal_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);
            var goalWork = new GoalWork(mockGoalRepository.Object);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create
        [Fact]
        public void Create_Success_GoalModelItem()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.Create(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            GoalModel result = goalWork.Create("test name");

            //Assert
            Assert.NotNull(result);
            mockGoalRepository.Verify(v => v.Create(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);
            mockGoalRepository.Setup(s => s.Update(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            goalWork.Update(Guid.NewGuid(), "test name", true);

            //Assert
            mockGoalRepository.Verify(v => v.Update(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        [Fact]
        public void Update_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            var goalWork = new GoalWork(mockGoalRepository.Object);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Update(Guid.NewGuid(), "test name", true));
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns(testDataItem);
            mockGoalRepository.Setup(s => s.Remove(It.IsAny<Guid>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            goalWork.Delete(Guid.NewGuid());

            //Assert
            mockGoalRepository.Verify(v => v.Remove(It.IsAny<Guid>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        [Fact]
        public void Delete_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoal(It.IsAny<Guid>())).Returns((GoalModel)null);

            var goalWork = new GoalWork(mockGoalRepository.Object);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Delete(Guid.NewGuid()));
        }
        #endregion

        #region Done
        [Fact]
        public void Done_Success()
        {
            //Arrange
            GoalModel testDataItem = testDataItems.First();

            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);
            mockGoalRepository.Setup(s => s.Update(It.IsAny<GoalModel>())).Verifiable();
            mockGoalRepository.Setup(s => s.Save()).Verifiable();

            var goalWork = new GoalWork(mockGoalRepository.Object);

            // Act
            goalWork.Done(testDataItem.Id, !testDataItem.Done);

            //Assert
            mockGoalRepository.Verify(v => v.Update(It.IsAny<GoalModel>()), Times.Once);
            mockGoalRepository.Verify(v => v.Save(), Times.Once);
        }

        [Fact]
        public void Done_IdNotFound_KeyNotFoundException()
        {
            //Arrange
            var mockGoalRepository = new Mock<IGoalRepository<GoalModel>>();
            mockGoalRepository.Setup(s => s.GetGoals()).Returns(testDataItems);

            var goalWork = new GoalWork(mockGoalRepository.Object);

            //Act & Assert
            Assert.Throws<KeyNotFoundException>(() => goalWork.Done(Guid.NewGuid(), true));
        }
        #endregion
    }
}
