using Data.Models.UserDb;
using Moq;
using Services.Repositories;
using Services.Services;
using Services.UnitOfWork;

namespace Tests
{
    public class UserServiceTests
    {
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);

            _userService = new UserService(_unitOfWorkMock.Object);
        }
        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User> { new User { Id = 1, Name = "John" } };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        // Additional tests
        [Test]
        public async Task GetUserByIdAsync_ReturnsUserById()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public async Task AddUserAsync_AddsUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };

            // Act
            await _userService.AddUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddAsync(user), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "John" };

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(user), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            const int userId = 1;

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }
    }
}
