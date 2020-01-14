using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmartHouse.Business.Data;
using SmartHouse.Domain.Core;
using SmartHouse.Infrastructure.Data;
using System.Linq;

namespace TestAPI
{
    public class GoalTests
    {
        private DbContextOptions<ActContext> options;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ActContext>()
            .UseInMemoryDatabase(databaseName: "SmartHouseDataBase")
            .Options;
        }

        [Test]
        public void GetGoalsTest()
        {
            using (var context = new ActContext(options))
            {
                context.Goals.RemoveRange(context.Goals.ToList());

                context.Goals.Add(new Goal { Id = 1, Name = "One" });
                context.Goals.Add(new Goal { Id = 2, Name = "Two" });
                context.SaveChanges();

                var goalWork = new GoalWork(context);
                var items = goalWork.GetGoals();

                Assert.AreEqual(2, items.Count());
            }

            //using (var context = new ActContext(options))
            //{
            //    var goalRepository = new GoalRepository(context);
            //    var count = goalRepository.GetGoals().Count();

            //    Assert.AreEqual(5, count);
            //}



            

            // Assert.Pass();
        }
    }
}