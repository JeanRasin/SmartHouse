using Bogus;
using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using Moq;
using RepositoryTest.Helpers;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RepositoryTest
{
    public class GoalRepositoryTest
    {
        public GoalRepositoryTest()
        {

        }

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

        /// <summary>
        /// https://github.com/rgvlee/EntityFrameworkCore.Testing
        /// </summary>
        [Fact]
        public void Repository_Create_Item()
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


            var mockedDbContext = Create.MockedDbContextFor<GoalContext>();
            var hh = mockedDbContext.Goals;

            int count = mockedDbContext.Goals.Count();

            //var goalSetMock = MockDbSet(testData);
            //goalSetMock.Setup(x => x.Add(itemTestData));

            // var tt = new InternalEntityEntry()

            //var entityEntryMock = new Mock<EntityEntry>(It.IsAny<InternalEntityEntry>());
            //entityEntryMock.Setup(x => x.Entity).Returns(itemTestData);

            //var dbContext = new Mock<IGoalContext>();
            //dbContext.Setup(m => m.Entry(It.IsAny<GoalModel>())).Returns(entityEntryMock.Object);

            var repository = new GoalRepository(mockedDbContext);
            repository.Create(itemTestData);

            mockedDbContext.SaveChanges();

            //dbContext.Verify(v => v.Entry(It.IsAny<GoalModel>()), Times.Once);

            // Assert.Equal(itemTestData, result);

            var dbContextMock = Mock.Get(mockedDbContext);

            dbContextMock.Verify(v => v.Entry(itemTestData), Times.Once, "Entry was not called.");
            Assert.Equal(count + 1, mockedDbContext.Goals.Count());//, "No entry has been added."
        }
    }
}