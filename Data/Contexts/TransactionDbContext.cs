using Microsoft.EntityFrameworkCore;
using Data.Models.TransactionDb;

namespace Data.Contexts
{
    public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { Id = 1, UserId = 1, ProductId = 2, TransactionDate = DateTime.Today, Status = "Done"},
                new Transaction { Id = 2, UserId = 2, ProductId = 1, TransactionDate = DateTime.Today, Status = "New" }
            );
        }
    }
}
