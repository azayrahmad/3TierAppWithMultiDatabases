using Microsoft.EntityFrameworkCore;
using Data.Models.TransactionDb;

namespace Data.Contexts
{
    public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=transaction.db");
        }
    }
}
