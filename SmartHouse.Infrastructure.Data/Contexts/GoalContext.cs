using Bogus;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;
using System;
using System.Collections.Generic;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalContext : DbContext
    {
        public virtual DbSet<GoalModel> Goals { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options) : base(options)//, List<GoalModel> creatingData = null
        {
            // Create a database on first call.
          //  Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Randomizer.Seed = new Random(1338);

        //    var goalModel = new Faker<GoalModel>()
        //         .RuleFor(o => o.Id, f => f.Random.Guid())
        //         .RuleFor(o => o.Name, f => f.Random.Words(3))
        //         .RuleFor(o => o.DateCreate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
        //         .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
        //         .RuleFor(o => o.Done, f => f.Random.Bool())
        //         .Generate(40);

        //    modelBuilder
        //        .Entity<GoalModel>()
        //        .HasData(goalModel);
        //}
    }
}
