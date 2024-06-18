using Moq;
using Services.DTOs;
using Services.UnitOfWork;
using Data.Models.ProductDb;
using Data.Models.UserDb;
using Data.Models.TransactionDb;
using Services.Services;

namespace Tests.Services.Services
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private TransactionService _transactionService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionService = new TransactionService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllTransactionsAsync_ShouldReturnTransactionDtos()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, UserId = 1, ProductId = 1, TransactionDate = DateTime.Now, Status = "Completed" },
                new Transaction { Id = 2, UserId = 2, ProductId = 2, TransactionDate = DateTime.Now, Status = "Pending" }
            };

            var users = new List<User>
            {
                new User { Id = 1, Name = "User1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User2", Email = "user2@example.com" }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };

            _unitOfWorkMock.Setup(uow => uow.Transactions.GetAllAsync()).ReturnsAsync(transactions);
            _unitOfWorkMock.Setup(uow => uow.Users.GetAllAsync()).ReturnsAsync(users);
            _unitOfWorkMock.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _transactionService.GetAllTransactionsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("User1", result.First().UserName);
            Assert.AreEqual("Product1", result.First().ProductName);
        }

        [Test]
        public async Task GetTransactionByIdAsync_ShouldReturnTransactionDto()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, TransactionDate = DateTime.Now, Status = "Completed" };
            var user = new User { Id = 1, Name = "User1", Email = "user1@example.com" };
            var product = new Product { Id = 1, Name = "Product1" };

            _unitOfWorkMock.Setup(uow => uow.Transactions.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transaction);
            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(uow => uow.Products.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("User1", result.UserName);
            Assert.AreEqual("Product1", result.ProductName);
        }

        [Test]
        public void AddTransactionAsync_ShouldThrowException_WhenUserOrProductNotExists()
        {
            // Arrange
            var transactionDto = new TransactionDto
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                TransactionDate = DateTime.Now,
                Status = "Completed"
            };

            _unitOfWorkMock.Setup(uow => uow.Users.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);
            _unitOfWorkMock.Setup(uow => uow.Products.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _transactionService.AddTransactionAsync(transactionDto), "User or Product does not exist.");
        }

        [Test]
        public async Task UpdateTransactionAsync_ShouldUpdateTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, TransactionDate = DateTime.Now, Status = "Completed" };
            var transactionDto = new TransactionDto
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                TransactionDate = DateTime.Now,
                Status = "Pending"
            };

            _unitOfWorkMock.Setup(uow => uow.Transactions.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transaction);

            // Act
            await _transactionService.UpdateTransactionAsync(transactionDto);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Transactions.UpdateAsync(It.Is<Transaction>(t => t.Status == "Pending")), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteTransactionAsync_ShouldDeleteTransaction()
        {
            // Arrange
            _unitOfWorkMock.Setup(uow => uow.Transactions.DeleteAsync(It.IsAny<int>())).Verifiable();

            // Act
            await _transactionService.DeleteTransactionAsync(1);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Transactions.DeleteAsync(It.Is<int>(id => id == 1)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}
