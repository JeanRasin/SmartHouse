using System;
using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;

namespace SmartHouse.Infrastructure.Data
{
    public class ActContext : DbContext
    {
        public DbSet<Goal> Goals { get; set; }

        public ActContext(DbContextOptions<ActContext> options) : base(options)
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        public ActContext()
        {
            Database.EnsureCreated(); // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Goal>().HasData(new Goal[] {
                new Goal{Id=1,Name="iman"},
                new Goal{Id=2,Name="Alex"},
                new Goal{Id=3,Name="333"},
            });
        }

    }
}
