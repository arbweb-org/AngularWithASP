using Microsoft.EntityFrameworkCore;
using angularwithasp.server.Models;

namespace angularwithasp.server.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        }

        public DbSet<User> Users { get; set; }
    }
}