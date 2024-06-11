using Data.Models.ProductDb;
using Data.Models.TransactionDb;
using Data.Models.UserDb;
using Moq;
using Services;
using Services.Repositories;
using Services.UnitOfWork;

namespace Tests
{
    public class TransactionServiceTests
    {
        private Mock<IRepository<Transaction>> _transactionRepositoryMock;
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IRepository<Product>> _productRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private TransactionService _transactionService;

        [SetUp]
        public void SetUp()
        {
            _transactionRepositoryMock = new Mock<IRepository<Transaction>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.Transactions).Returns(_transactionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);

            _transactionService = new TransactionService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllTransactionsAsync_ReturnsAllTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
        {
            new Transaction { Id = 1, UserId = 1, ProductId = 1, Status = "Completed", TransactionDate = DateTime.Now },
            new Transaction { Id = 2, UserId = 2, ProductId = 2, Status = "Pending", TransactionDate = DateTime.Now }
        };

            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetAllTransactionsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetTransactionByIdAsync_ExistingId_ReturnsTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, Status = "Completed", TransactionDate = DateTime.Now };
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(transaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(1);

            // Assert
            Assert.That(result.Status, Is.EqualTo("Completed"));
        }

        [Test]
        public async Task AddTransactionAsync_ValidTransaction_AddsTransaction()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, Status = "Completed", TransactionDate = DateTime.Now };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1, Name = "Alice" });
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new Product { Id = 1, Name = "Product A" });
            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction)).Returns(Task.CompletedTask);

            // Act
            await _transactionService.AddTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(transaction), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateTransactionAsync_ExistingTransaction_UpdatesTransaction()
        {
            // Arrange
            var existingTransaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, Status = "New", TransactionDate = DateTime.Now };
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 1, Status = "Completed", TransactionDate = DateTime.Now };

            _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(existingTransaction.Id)).ReturnsAsync(existingTransaction);
            _transactionRepositoryMock.Setup(repo => repo.UpdateAsync(existingTransaction)).Returns(Task.CompletedTask);

            // Act
            await _transactionService.UpdateTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.UpdateAsync(existingTransaction), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteTransactionAsync_ExistingId_DeletesTransaction()
        {
            // Arrange
            _transactionRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _transactionService.DeleteTransactionAsync(1);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void AddTransactionAsync_InvalidUser_ThrowsException()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 999, ProductId = 1, Status = "Completed", TransactionDate = DateTime.Now };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _transactionService.AddTransactionAsync(transaction));
            Assert.That(ex.Message, Is.EqualTo("User or Product does not exist."));
        }

        [Test]
        public void AddTransactionAsync_InvalidProduct_ThrowsException()
        {
            // Arrange
            var transaction = new Transaction { Id = 1, UserId = 1, ProductId = 999, Status = "Completed", TransactionDate = DateTime.Now };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1, Name = "Alice" });
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Product)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _transactionService.AddTransactionAsync(transaction));
            Assert.That(ex.Message, Is.EqualTo("User or Product does not exist."));
        }
    }
}
