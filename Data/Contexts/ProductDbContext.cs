using Microsoft.EntityFrameworkCore;
using Data.Models.ProductDb;

namespace Data.Contexts
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=product.db");
        }

    }
}
