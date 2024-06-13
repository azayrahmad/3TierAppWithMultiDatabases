using Services.DTOs;
using Services.UnitOfWork;

namespace Services.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(p => p.ToDto()); ;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user.ToDto();
        }
        public async Task<UserDto> GetUserByIdWithTransactionsAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            var transactions = await _unitOfWork.Transactions.GetAllByUserIdAsync(id);
            var products = await _unitOfWork.Products.GetAllAsync();
            var transactionDtos = transactions.Select(t =>
            {
                var product = products.FirstOrDefault(p => p.Id == t.ProductId);
                return t.ToDto(user, product);
            });

            var userDto = user.ToDto();
            userDto.Transactions = transactionDtos;
            return userDto;
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            await _unitOfWork.Users.AddAsync(userDto.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            await _unitOfWork.Users.UpdateAsync(user.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
