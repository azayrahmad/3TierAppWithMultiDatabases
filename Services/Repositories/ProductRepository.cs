using Data.Contexts;
using Data.Models.ProductDb;

namespace Services.Repositories
{
    public class ProductRepository(ProductDbContext context) : 
        GenericRepository<Product, ProductDbContext>(context), IProductRepository
    {
    }
}
