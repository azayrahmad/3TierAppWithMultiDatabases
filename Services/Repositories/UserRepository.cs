using Data.Contexts;
using Data.Models.UserDb;

namespace Services.Repositories
{
    public class UserRepository(UserDbContext context) : 
        GenericRepository<User, UserDbContext>(context), IUserRepository
    {
    }
}
