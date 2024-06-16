using Microsoft.EntityFrameworkCore;
using Data.Models.TransactionDb;

namespace Data.Contexts
{
    public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }
    }
}
