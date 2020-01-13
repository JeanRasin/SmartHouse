using Microsoft.EntityFrameworkCore;
using SmartHouse.Infrastructure.Data;

namespace ConsoleAPI
{
    public class ApplicationContext : ActContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres");
        }
    }
}
