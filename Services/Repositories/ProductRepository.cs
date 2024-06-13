using Data.Contexts;
using Data.Models.ProductDb;
using Services.Repositories.Interfaces;

namespace Services.Repositories
{
    public class ProductRepository(ProductDbContext context) : 
        Repository<Product, ProductDbContext>(context), IProductRepository
    {
    }
}
