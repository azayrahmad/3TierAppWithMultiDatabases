using Microsoft.EntityFrameworkCore;
using Data.Models.ProductDb;

namespace Data.Contexts
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            var category1 = new Category { Id = 1, Name = "Category One" };
            var category2 = new Category { Id = 2, Name = "Category Two" };
            modelBuilder.Entity<Category>().HasData(
                category1,
                category2
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "First Product", CategoryId = 1, Price = 100 },
                new Product { Id = 2, Name = "Second Product", CategoryId = 2, Price = 200 }
            );
        }

    }
}
