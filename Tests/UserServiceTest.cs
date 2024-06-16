using Data.Models.UserDb;
using Services.DTOs;
using Moq;
using Services.Services;
using Services.UnitOfWork;
using Services;
using Data.Models.TransactionDb;

namespace Tests
{
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userService = new UserService(_unitOfWorkMock.Object);
        }
        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1, Name = "John" } };
            _unitOfWorkMock.Setup(uow => uow.Users.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo(users.First().Name));
        }

        // Additional tests
        [Test]
        public async Task GetUserByIdAsync_ReturnsUserById()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };
            _unitOfWorkMock.Setup(repo => repo.Users.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
        }

        [Test]
        public async Task AddUserAsync_AddsUser()
        {
            // Arrange
            var user = new UserDto { Id = 1, Name = "John", Email = "john@test.com" };
            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(It.Is<int>(id => id == user.Id))).ReturnsAsync(user.ToEntity());

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Users.AddAsync(It.Is<User>(u => u.Id == user.Id)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };
            var updatedUserDto = user.ToDto();
            updatedUserDto.Name = "Jane";
            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);


            // Act
            await _userService.UpdateUserAsync(updatedUserDto);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Users.UpdateAsync(It.Is<User>(u => u.Id == user.Id && u.Name == updatedUserDto.Name)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            const int userId = 1;
            _unitOfWorkMock.Setup(uow => uow.Users.DeleteAsync(It.IsAny<int>())).Verifiable();
            _unitOfWorkMock.Setup(uow => uow.Transactions.GetByIdAsync(userId)).ReturnsAsync((Transaction)null);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Users.DeleteAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);


        }
    }
}
