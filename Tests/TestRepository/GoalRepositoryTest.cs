using Bogus;
using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Moq;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RepositoryTest
{
    /// <summary>
    /// https://github.com/rgvlee/EntityFrameworkCore.Testing
    /// </summary>
    [CollectionDefinition("Goal repository")]
    public class GoalRepositoryTest
    {
        private static readonly GoalModel _itemTestData = GetTestData();
        private readonly GoalContext _mockedDbContext;
        private readonly GoalRepository _repository;
        private readonly Mock<GoalContext> _dbContextMock;

        public GoalRepositoryTest()
        {
            _mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            _repository = new GoalRepository(_mockedDbContext);
            _dbContextMock = Mock.Get(_mockedDbContext);
        }

        static GoalModel GetTestData()
        {
            // Random constant.
            Randomizer.Seed = new Random(1338);

            GoalModel result = new Faker<GoalModel>()
          .StrictMode(true)
          .RuleFor(o => o.Id, f => f.Random.Uuid())
          .RuleFor(o => o.Name, f => f.Random.Words(3))
          .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
          .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
          .RuleFor(o => o.Done, f => f.Random.Bool())
          .Generate();

            return result;
        }

        #region Just in case 
        /// <summary>
        /// Mock dbSet. todo: возможно пригадится.
        /// </summary>
        /// <remarks>
        /// https://stackoverflow.com/questions/37630564/how-to-mock-up-dbcontext
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> list) where T : class, new()
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryableList.GetEnumerator());
            // dbSetMock.Setup(x => x.Create()).Returns(new T());

            return dbSetMock;
        }
        #endregion

        [Fact(Skip = "Thats how you ignore a test", DisplayName = "To Ignore")]//todo: для запоминания
        public void ToIgnore()
        {
            Assert.False(true);
        }

        #region GetGoals
        /// <summary>
        /// Get goals.
        /// </summary>
        [Fact]
        public void GetGoals_GoalModelItems()
        {
            // Act
            IEnumerable<GoalModel> items = _repository.GetGoals();

            // Assert
            _dbContextMock.VerifyGet(v => v.Goals, "Property was not called.");
            Assert.Equal(items.Count(), _mockedDbContext.Goals.Count());
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
            _mockedDbContext.Goals.Add(_itemTestData);

            // Act
            GoalModel result = _repository.GetGoal(_itemTestData.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_itemTestData, result);
        }

        /// <summary>
        /// Get goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void GetGoal_KeyNotFound_KeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create goal.
        /// </summary>
        [Fact]
        public void Create_Success()
        {
            // Act
            _repository.Create(_itemTestData);

            // Assert
            EntityState itemState = _mockedDbContext.Entry(_itemTestData).State;
            _dbContextMock.Verify(v => v.Entry(_itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Added, itemState);
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove goal.
        /// </summary>
        [Fact]
        public void Remove_Success()
        {
            // Arrange
            _mockedDbContext.Goals.Add(_itemTestData);

            // Act
            _repository.Remove(_itemTestData.Id);

            // Assert
            EntityState itemState = _mockedDbContext.Entry(_itemTestData).State;
            _dbContextMock.Verify(v => v.Entry(_itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Deleted, itemState);
        }

        /// <summary>
        /// Remove goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void Remove_KeyNotFound_KeyNotFoundException()
        {
            // Act && Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Remove(Guid.NewGuid()));
        }
        #endregion

        #region Update
        /// <summary>
        /// Update goal.
        /// </summary>
        [Fact]
        public void Update_Success()
        {
            // Arrange
            _repository.Create(_itemTestData);

            // Act
            _repository.Update(_itemTestData);

            // Assert
            EntityState itemState = _mockedDbContext.Entry(_itemTestData).State;
            _dbContextMock.Verify(v => v.Entry(_itemTestData), Times.Exactly(3), "Entry was not called.");
            Assert.Equal(EntityState.Modified, itemState);
        }

        /// <summary>
        /// Update goal key id not found. Throw exception KeyNotFoundException.
        /// </summary>
        [Fact]
        public void Update_KeyNotFound_KeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(_itemTestData));
        }
        #endregion
    }
}