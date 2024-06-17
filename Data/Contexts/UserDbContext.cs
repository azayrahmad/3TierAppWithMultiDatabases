using Microsoft.EntityFrameworkCore;
using Data.Models.UserDb;

namespace Data.Contexts
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Alice", Email = "alice@user1.com" },
                new User { Id = 2, Name = "Bob", Email = "bob@user2.com" }
            );
        }
    }
}
