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
        private List<GoalModel> testData;
        public GoalRepositoryTest()
        {
            testData = new Faker<GoalModel>()
          .StrictMode(true)
          .RuleFor(o => o.Id, f => f.Random.Uuid())
          .RuleFor(o => o.Name, f => f.Random.Words(3))
          .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
          .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
          .RuleFor(o => o.Done, f => f.Random.Bool())
          .Generate(1);
        }

        #region Just in case
        /// <summary>
        /// Mock dbSet.
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

        /*
        [Fact]
        public void Repository_GetGoals_Items()
        {
            var testFakerData = new Faker<GoalModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid())
                .RuleFor(o => o.Name, f => f.Random.Words(3))
                .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.Done, f => f.Random.Bool());

            List<GoalModel> testData = testFakerData.Generate(10);

            var goalSetMock = MockDbSet(testData);

            var dbContext = new Mock<IGoalContext>();
            dbContext.Setup(m => m.Goals).Returns(goalSetMock.Object);

            var repository = new GoalRepository(dbContext.Object);
            List<GoalModel> result = repository.GetGoals().ToList();

            Assert.Equal(testData, result);
        }

        [Fact]
        public void Repository_GetGoal_Item()
        {
            var testFakerData = new Faker<GoalModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid())
                .RuleFor(o => o.Name, f => f.Random.Words(3))
                .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.Done, f => f.Random.Bool());

            List<GoalModel> testData = testFakerData.Generate(1);

            GoalModel itemTestData = testData.Single();

            var goalSetMock = MockDbSet(testData);
            goalSetMock.Setup(x => x.Find(itemTestData.Id)).Returns(itemTestData);

            var dbContext = new Mock<IGoalContext>();
            dbContext.Setup(m => m.Goals).Returns(goalSetMock.Object);

            var repository = new GoalRepository(dbContext.Object);
            GoalModel result = repository.GetGoal(itemTestData.Id);

            Assert.Equal(itemTestData, result);
        }
        */

        [Fact(Skip = "Thats how you ignore a test", DisplayName = "To Ignore")]//todo: для запоминания
        public void ToIgnore()
        {
            Assert.False(true);
        }

        #region GetGoals
        [Fact]
        public void Repository_GetGoals_Items()
        {
            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            var repository = new GoalRepository(mockedDbContext);
            var items = repository.GetGoals();

            var dbContextMock = Mock.Get(mockedDbContext);

            dbContextMock.VerifyGet(v => v.Goals, "Property was not called.");
            Assert.Equal(items.Count(), mockedDbContext.Goals.Count());
        }
        #endregion

        #region GetGoal 
        [Fact]
        public void Repository_GetGoal_Item()
        {
            //var testFakerData = new Faker<GoalModel>()
            // .StrictMode(true)
            // .RuleFor(o => o.Id, f => f.Random.Uuid())
            // .RuleFor(o => o.Name, f => f.Random.Words(3))
            // .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            // .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            // .RuleFor(o => o.Done, f => f.Random.Bool());

            //List<GoalModel> testData = testFakerData.Generate(1);
            GoalModel itemTestData = testData.Single();

            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            mockedDbContext.Goals.Add(itemTestData);

            var repository = new GoalRepository(mockedDbContext);
            GoalModel result = repository.GetGoal(itemTestData.Id);

            Assert.NotNull(result);
            Assert.Equal(itemTestData, result);
        }

        [Fact]
        public void Repository_GetGoal_KeyNotFoundException()
        {
            GoalContext mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            var repository = new GoalRepository(mockedDbContext);

            Assert.Throws<KeyNotFoundException>(() => repository.GetGoal(Guid.NewGuid()));
        }
        #endregion

        #region Create 
        [Fact]
        public void Repository_Create_Item()
        {
            //var testFakerData = new Faker<GoalModel>()
            //    .StrictMode(true)
            //    .RuleFor(o => o.Id, f => f.Random.Uuid())
            //    .RuleFor(o => o.Name, f => f.Random.Words(3))
            //    .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.Done, f => f.Random.Bool());

            //List<GoalModel> testData = testFakerData.Generate(1);

            GoalModel itemTestData = testData.Single();

            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            var repository = new GoalRepository(mockedDbContext);
            repository.Create(itemTestData);

            var dbContextMock = Mock.Get(mockedDbContext);
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;

            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Added, itemState);
        }
        #endregion

        #region Remove
        [Fact]
        public void Repository_Remove_Item()
        {
            //var testFakerData = new Faker<GoalModel>()
            //    .StrictMode(true)
            //    .RuleFor(o => o.Id, f => f.Random.Uuid())
            //    .RuleFor(o => o.Name, f => f.Random.Words(3))
            //    .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.Done, f => f.Random.Bool());

            //List<GoalModel> testData = testFakerData.Generate(1);
            GoalModel itemTestData = testData.Single();

            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            mockedDbContext.Goals.Add(itemTestData);

            var repository = new GoalRepository(mockedDbContext);
            repository.Remove(itemTestData.Id);

            var dbContextMock = Mock.Get(mockedDbContext);
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;

            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(2), "Entry was not called.");
            Assert.Equal(EntityState.Deleted, itemState);
        }

        [Fact]
        public void Repository_Remove_KeyNotFoundException()
        {
            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            var repository = new GoalRepository(mockedDbContext);

            Assert.Throws<KeyNotFoundException>(() => repository.Remove(Guid.NewGuid()));
        }
        #endregion

        #region Update
        [Fact]
        public void Repository_Update_Item()
        {
            //var testFakerData = new Faker<GoalModel>()
            //    .StrictMode(true)
            //    .RuleFor(o => o.Id, f => f.Random.Uuid())
            //    .RuleFor(o => o.Name, f => f.Random.Words(3))
            //    .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.Done, f => f.Random.Bool());

            //List<GoalModel> testData = testFakerData.Generate(1);
            GoalModel itemTestData = testData.Single();

            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            var repository = new GoalRepository(mockedDbContext);
            repository.Create(itemTestData);
            repository.Update(itemTestData);

            var dbContextMock = Mock.Get(mockedDbContext);
            EntityState itemState = mockedDbContext.Entry(itemTestData).State;

            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Exactly(3), "Entry was not called.");
            Assert.Equal(EntityState.Modified, itemState);
        }

        [Fact]
        public void Repository_Update_KeyNotFoundException()
        {
            //var testFakerData = new Faker<GoalModel>()
            //    .StrictMode(true)
            //    .RuleFor(o => o.Id, f => f.Random.Uuid())
            //    .RuleFor(o => o.Name, f => f.Random.Words(3))
            //    .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
            //    .RuleFor(o => o.Done, f => f.Random.Bool());

            //List<GoalModel> testData = testFakerData.Generate(1);
            GoalModel itemTestData = testData.Single();

            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();

            var repository = new GoalRepository(mockedDbContext);

            Assert.Throws<KeyNotFoundException>(() => repository.Update(itemTestData));
        }
        #endregion
    }
}