using Bogus;
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

        [Fact]
        public void Repository_Get_List()
        {
            var data = new Faker<GoalModel>()
                .StrictMode(true)
                .RuleFor(o => o.Id, f => f.Random.Uuid())
                .RuleFor(o => o.Name, f => f.Random.Words(3))
                .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.Done, f => f.Random.Bool());

            //            var data = new[]
            //{
            //    new GoalModel(),
            //    new GoalModel(),
            //    new GoalModel()
            //}.AsQueryable();

         //   var mock = new Mock<DbSet<GoalModel>>();

            // This line is new
            //mock.As<IAsyncEnumerable<GoalModel>>()
            //    .Setup(x => x.GetAsyncEnumerator())
            //    .Returns(new TestDbAsyncEnumerator<Foo>(data.GetEnumerator()));

            //// this line is updated
            //mock.As<IQueryable<Foo>>()
            //    .Setup(x => x.Provider)
            //    .Returns(new Test DbAsyncQueryProvider<Foo>(data.Provider));

            //mock.As<IQueryable<Foo>>()
            //    .Setup(x => x.Expression)
            //    .Returns(data.Expression);

            //mock.As<IQueryable<Foo>>()
            //    .Setup(x => x.ElementType)
            //    .Returns(data.ElementType);

            //mock.As<IQueryable<Foo>>()
            //    .Setup(x => x.GetEnumerator())
            //    .Returns(data.GetEnumerator());

            var result = data.Generate(1);

            //var mockSet = new Mock<DbSet<GoalModel>>();
            //mockSet.As<IQueryable<GoalModel>>().Setup(m => m.Provider).Returns(result.AsQueryable().Provider);
            //mockSet.As<IQueryable<GoalModel>>().Setup(m => m.Expression).Returns(result.AsQueryable().Expression);
            //mockSet.As<IQueryable<GoalModel>>().Setup(m => m.ElementType).Returns(result.AsQueryable().ElementType);
            //mockSet.As<IQueryable<GoalModel>>().Setup(m => m.GetEnumerator()).Returns(result.AsQueryable().GetEnumerator());
            //mockSet.Setup(m => m.Add(It.IsAny<GoalModel>())).Callback<GoalModel>(result.Add);
            //return mockSet;

            var mockSet = MockHelper.GetMockDbSet(result);

            /*
            var relationalCommand = new Mock<IRelationalCommand>();
            relationalCommand.Setup(m => m.ExecuteNonQuery(It.IsAny<RelationalCommandParameterObject>())).Returns(1);

            var rawSqlCommand = new Mock<RawSqlCommand>(MockBehavior.Strict, relationalCommand.Object, new Dictionary<string, object>());
            rawSqlCommand.Setup(m => m.RelationalCommand).Returns(() => relationalCommand.Object);
            rawSqlCommand.Setup(m => m.ParameterValues).Returns(new Dictionary<string, object>());

            var rawSqlCommandBuilder = new Mock<IRawSqlCommandBuilder>();
            rawSqlCommandBuilder.Setup(m => m.Build(It.IsAny<string>(), It.IsAny<IEnumerable<object>>())).Returns(rawSqlCommand.Object);

            var databaseFacade = new Mock<DatabaseFacade>(context.Object);
            databaseFacade.As<IInfrastructure<IServiceProvider>>().Setup(m => m.Instance.GetService(It.Is<Type>(t => t == typeof(IConcurrencyDetector)))).Returns(new Mock<IConcurrencyDetector>().Object);
            databaseFacade.As<IInfrastructure<IServiceProvider>>().Setup(m => m.Instance.GetService(It.Is<Type>(t => t == typeof(IRawSqlCommandBuilder)))).Returns(rawSqlCommandBuilder.Object);
            databaseFacade.As<IInfrastructure<IServiceProvider>>().Setup(m => m.Instance.GetService(It.Is<Type>(t => t == typeof(IRelationalConnection)))).Returns(new Mock<IRelationalConnection>().Object);
            */

            var context = new Mock<GoalContext>();
          
            context.Setup(l => l.Set<GoalModel>()).Returns(mockSet.Object);
            context.Setup(l => l.Goals).Returns(mockSet.Object);

          //  context.Setup(l => l.Goals.Find()).Returns(result.First());

            var repository = new GoalRepository(context.Object);

            GoalModel item = result.Single();
           var rr= repository.GetGoal(item.Id);

            Assert.True(true);
        }
    }
}
