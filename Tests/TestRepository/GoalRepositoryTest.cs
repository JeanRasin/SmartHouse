// EntityFrameworkCore.Testing - https://github.com/rgvlee/EntityFrameworkCore.Testing

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
    [Collection("Goal repository")]
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

        /// <summary>
        /// Get test data.
        /// </summary>
        /// <returns></returns>
        private static GoalModel GetTestData()
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

        #region GetGoals

        /// <summary>
        /// Get goals.
        /// </summary>
        [Trait("Get Goals", "Success")]
        [Fact]
        public void GetGoals_GoalModelItems()
        {
            // Act
            IEnumerable<GoalModel> items = _repository.GetGoals();

            // Assert
            _dbContextMock.VerifyGet(v => v.Goals, "Property was not called.");
            Assert.Equal(items.Count(), _mockedDbContext.Goals.Count());
        }

        #endregion GetGoals

        #region GetGoal

        /// <summary>
        /// Get goal by id.
        /// </summary>
        [Fact]
        [Trait("Get Goal By Id", "Success")]
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
        [Trait("Get Goal By Id", "KeyNotFoundException")]
        public void GetGoal_KeyNotFound_KeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.GetGoal(Guid.NewGuid()));
        }

        #endregion GetGoal

        #region Create

        /// <summary>
        /// Create goal.
        /// </summary>
        [Fact]
        [Trait("Create", "Success")]
        public void Create_Success()
        {
            // Act
            _repository.Create(_itemTestData);

            // Assert
            EntityState itemState = _mockedDbContext.Entry(_itemTestData).State;
            _dbContextMock.Verify(v => v.Entry(_itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Added, itemState);
        }

        #endregion Create

        #region Remove

        /// <summary>
        /// Remove goal.
        /// </summary>
        [Fact]
        [Trait("Remove", "Success")]
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
        [Trait("Remove", "KeyNotFoundException")]
        public void Remove_KeyNotFound_KeyNotFoundException()
        {
            // Act && Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Remove(Guid.NewGuid()));
        }

        #endregion Remove

        #region Update

        /// <summary>
        /// Update goal.
        /// </summary>
        [Fact]
        [Trait("Update", "Success")]
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
        [Trait("Remove", "KeyNotFoundException")]
        public void Update_KeyNotFound_KeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(_itemTestData));
        }

        #endregion Update
    }
}