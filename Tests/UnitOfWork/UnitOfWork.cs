using NUnit.Framework;
using Moq;
using Services.UnitOfWork;
using Services.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Tests.Services.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            _unitOfWorkMock.SetupGet(u => u.Users).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Products).Returns(_productRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Transactions).Returns(_transactionRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.Categories).Returns(_categoryRepositoryMock.Object);
        }

        [Test]
        public async Task CompleteAsync_CallsCompleteAsync()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await _unitOfWorkMock.Object.CompleteAsync();

            // Assert
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public void Dispose_CallsDispose()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Dispose()).Verifiable();

            // Act
            _unitOfWorkMock.Object.Dispose();

            // Assert
            _unitOfWorkMock.Verify(u => u.Dispose(), Times.Once);
        }
    }
}
