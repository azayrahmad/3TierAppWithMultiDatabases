using Data.Contexts;
using Data.Models.ProductDb;
using Services.Repositories.Interfaces;

namespace Services.Repositories
{
    public class CategoryRepository(ProductDbContext context) : 
        Repository<Category, ProductDbContext>(context), ICategoryRepository
    {
    }
}
