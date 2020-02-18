using System;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;
using Bogus;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SmartHouse.Infrastructure.Data
{
    public interface IGoalContext//todo:remove
    {
        DbSet<GoalModel> Goals { get; set; }
        EntityEntry Remove(object goal);
        EntityEntry Entry(object data);
        int SaveChanges();
        void Dispose();
    }

    public class GoalContext : DbContext, IGoalContext
    {
        public virtual DbSet<GoalModel> Goals { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options) : base(options)
        {
            // Create a database on first call.
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Randomizer.Seed = new Random(1338);

           var goalModel = new Faker<GoalModel>()
                .RuleFor(o => o.Id, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Random.Words(3))
                .RuleFor(o => o.DateCreate, f=> f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.Done, f => f.Random.Bool())
                .Generate(40);

            //List<GoalModel> goalModel = goalModelFakes.Generate(40);

            modelBuilder
                .Entity<GoalModel>()
                .HasData(goalModel);
        }
    }
}
