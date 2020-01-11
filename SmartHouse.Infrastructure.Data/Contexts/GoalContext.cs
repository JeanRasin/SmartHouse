using Microsoft.EntityFrameworkCore;
using SmartHouse.Domain.Core;

namespace SmartHouse.Infrastructure.Data
{
    public class GoalContext : DbContext
    {
        public virtual DbSet<Goal> Goals { get; set; }

        public GoalContext(DbContextOptions<GoalContext> options) : base(options)
        {
        }
    }
}