using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SmartHouse.Domain.Core;
using System;

namespace SmartHouseAPI
{
    public class GoalContext : SmartHouse.Infrastructure.Data.GoalContext
    {
        public GoalContext(DbContextOptions<SmartHouse.Infrastructure.Data.GoalContext> options, IWebHostEnvironment env) : base(options)
        {
            if (env.IsDevelopment())
            {
                // Create a database on first call.
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Randomizer.Seed = new Random(1338);

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