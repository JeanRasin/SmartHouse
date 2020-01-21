using System;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalContext : DbContext
    {
        private readonly string connectText;

        public DbSet<GoalModel> Goals { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options) : base(options)
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        public GoalContext()
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        public GoalContext(string connectText)
        {
            this.connectText = connectText;
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoalModel>().HasData(new GoalModel[] {
                new GoalModel{Id=1,Name="iman"},
                new GoalModel{Id=2,Name="Alex"},
                new GoalModel{Id=3,Name="555"},
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectText);
        }

    }
}
