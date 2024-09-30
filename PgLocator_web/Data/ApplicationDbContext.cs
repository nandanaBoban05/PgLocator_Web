using Microsoft.EntityFrameworkCore;
using PgLocator_web.Models;

namespace PgLocator_web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Pg> Pg { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Room> Room { get; set; }

    }
}