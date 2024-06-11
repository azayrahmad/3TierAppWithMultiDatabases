using Data.Models.UserDb;
using Services.UnitOfWork;

namespace Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _unitOfWork.Users.GetAllAsync();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
