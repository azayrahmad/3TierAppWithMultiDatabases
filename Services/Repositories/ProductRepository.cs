using Data.Contexts;
using Data.Models.ProductDb;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;

namespace Services.Repositories
{
    public class ProductRepository(ProductDbContext context) : 
        Repository<Product, ProductDbContext>(context), IProductRepository
    {
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public override async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
