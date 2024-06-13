using Data.Contexts;
using Data.Models.UserDb;
using Services.Repositories.Interfaces;

namespace Services.Repositories
{
    public class UserRepository(UserDbContext context) : 
        Repository<User, UserDbContext>(context), IUserRepository
    {
    }
}
