using Data.Models.ProductDb;
using Moq;
using Services;
using Services.Repositories;
using Services.UnitOfWork;

namespace Tests
{
    public class ProductServiceTests
    {
        private Mock<IRepository<Product>> _productRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        
            _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepositoryMock.Object);
            _productService = new ProductService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product1", Price = 10 } };
            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        // Additional tests
        [Test]
        public async Task GetProductByIdAsync_ReturnsProductById()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(product));
        }

        [Test]
        public async Task AddProductAsync_AddsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };

            // Act
            await _productService.AddProductAsync(product);

            // Assert
            _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };

            // Act
            await _productService.UpdateProductAsync(product);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            // Arrange
            const int productId = 1;

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }
    }
}
