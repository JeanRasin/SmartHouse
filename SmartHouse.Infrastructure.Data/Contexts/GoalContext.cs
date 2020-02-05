using System;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;
using Bogus;
using System.Collections.Generic;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalContext : DbContext
    {
       // private readonly string connectText;

        public DbSet<GoalModel> Goals { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options) : base(options)
        {
           // connectText = options.
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        public GoalContext()
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        //public GoalContext(string connectText)
        //{
        //    this.connectText = connectText;
        //    Database.EnsureCreated(); // создаем базу данных при первом обращении
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // Randomizer.Seed = new Random(1338);

            var goalModelFakes = new Faker<GoalModel>()
                .RuleFor(o => o.Id, f => f.Random.Guid())
                .RuleFor(o => o.Name, f => f.Random.Words(3))
                .RuleFor(o => o.DateCreate, f=> f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.DateUpdate, f => f.Date.Between(new DateTime(1997, 1, 1), new DateTime(1997, 2, 1)))
                .RuleFor(o => o.Done, f => f.Random.Bool());

            List<GoalModel> goalModel = goalModelFakes.Generate(5);

            modelBuilder
                .Entity<GoalModel>()
                .HasData(goalModel);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(connectText);
        //}

    }
}
