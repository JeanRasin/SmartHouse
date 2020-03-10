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
    public class MockedDbContextFixture : IDisposable
    {
        public GoalContext MockedDbContext { get; private set; }

        public MockedDbContextFixture()
        {
            MockedDbContext = Create.MockedDbContextFor<GoalContext>();
        }

        #region dispose
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    MockedDbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// https://github.com/rgvlee/EntityFrameworkCore.Testing
    /// </summary>
    [CollectionDefinition("Goal repository")]
    public class GoalRepositoryTest : IClassFixture<MockedDbContextFixture>
    {
        private static readonly GoalModel itemTestData = GetTestData();
        private readonly GoalContext mockedDbContext;
        private readonly GoalRepository repository;
        private readonly Mock<GoalContext> dbContextMock;

        public GoalRepositoryTest(MockedDbContextFixture mockedDbContextFixture)
        {
            // itemTestData = testData.Single();

             mockedDbContext = Create.MockedDbContextFor<GoalContext>();

           // mockedDbContext = mockedDbContextFixture.MockedDbContext;

            repository = new GoalRepository(mockedDbContext);
            dbContextMock = Mock.Get(mockedDbContext);
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
        [Fact]
        public void GetGoals_GoalModelItems()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            // var repository = new GoalRepository(mockedDbContext);

            // Act
            IEnumerable<GoalModel> items = repository.GetGoals();

            // Mock<GoalContext> dbContextMock = Mock.Get(mockedDbContext);

            // Assert
            dbContextMock.VerifyGet(v => v.Goals, "Property was not called.");
            Assert.Equal(items.Count(), mockedDbContext.Goals.Count());
        }
        #endregion

        #region GetGoal 
        [Fact]
        public void GetGoal_GoalModelItem()
        {
            //   GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            // Arrange
            mockedDbContext.Goals.Add(itemTestData);

            //var repository = new GoalRepository(mockedDbContext);
            // Act
            GoalModel result = repository.GetGoal(itemTestData.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itemTestData, result);
        }

        [Fact]
        public void GetGoal_KeyNotFound_KeyNotFoundException()
        {
            //  GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            //var repository = new GoalRepository(mockedDbContext);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create 
        [Fact]
        public void Create_Success()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            // Arrange
           // Mock<GoalContext> dbContextMock = Mock.Get(mockedDbContext);

            // var repository = new GoalRepository(mockedDbContext);

            // Act
            repository.Create(itemTestData);

            // Assert
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;
            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Added, itemState);
        }
        #endregion

        #region Remove
        [Fact]
        public void Remove_Success()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            // Arrange
           // Mock<GoalContext> dbContextMock = Mock.Get(mockedDbContext);
            mockedDbContext.Goals.Add(itemTestData);

            // var repository = new GoalRepository(mockedDbContext);

            // Act
            repository.Remove(itemTestData.Id);

            // Assert
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;
            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Deleted, itemState);
        }

        [Fact]
        public void Remove_KeyNotFound_KeyNotFoundException()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            // var repository = new GoalRepository(mockedDbContext);

            // Act && Assert
            Assert.Throws<KeyNotFoundException>(() => repository.Remove(Guid.NewGuid()));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            // var repository = new GoalRepository(mockedDbContext);

            // Arrange
           // Mock<GoalContext> dbContextMock = Mock.Get(mockedDbContext);
            repository.Create(itemTestData);

            // Act
            repository.Update(itemTestData);

            // Assert
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;
            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(3), "Entry was not called.");
            Assert.Equal(EntityState.Modified, itemState);
        }

        [Fact]
        public void Update_KeyNotFound_KeyNotFoundException()
        {
            // GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            //  var repository = new GoalRepository(mockedDbContext);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.Update(itemTestData));
        }
        #endregion
    }
}