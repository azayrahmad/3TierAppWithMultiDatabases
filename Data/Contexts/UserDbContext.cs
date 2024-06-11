using Microsoft.EntityFrameworkCore;
using Data.Models.UserDb;

namespace Data.Contexts
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=user.db");
        }
    }
}
