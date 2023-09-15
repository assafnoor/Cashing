using Cashing.Models;
using Microsoft.EntityFrameworkCore;

namespace Cashing
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Driver> Drivers { get; set; }
    }
}
