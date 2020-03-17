using Bogus;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;
using System;

namespace SmartHouseAPI.Contexts
{
    public class GoalContext : SmartHouse.Infrastructure.Data.GoalContext
    {
        public GoalContext(DbContextOptions<SmartHouse.Infrastructure.Data.GoalContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var goalModel = new Faker<GoalModel>()
             .RuleFor(o => o.Id, f => f.Random.Guid())
             .RuleFor(o => o.Name, f => f.Random.Words(3))
             .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
             .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
             .RuleFor(o => o.Done, f => f.Random.Bool())
             .Generate(40);

            modelBuilder
                .Entity<GoalModel>()
                .HasData(goalModel);
        }
    }
}