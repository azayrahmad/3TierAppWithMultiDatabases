using Data.Models.ProductDb;
using Services.DTOs;
using Moq;
using Services;
using Services.UnitOfWork;
using Data.Contexts;
using Castle.Components.DictionaryAdapter.Xml;
using Services.Services;

namespace Tests
{
    public class ProductServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private ProductService _productService;
        private Mock<ProductDbContext> _productDbContextMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        
            _unitOfWorkMock.Setup(u => u.Products).Returns(_unitOfWorkMock.Object.Products);
            _productService = new ProductService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product1", Price = 10 } };
            _unitOfWorkMock.Setup(repo => repo.Products.GetAllAsync()).ReturnsAsync(products);

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
            _unitOfWorkMock.Setup(repo => repo.Products.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.That(result, Is.EqualTo(product.ToDto()));
        }

        [Test]
        public async Task AddProductAsync_AddsProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product1", Price = 10 };

            // Act
            await _productService.AddProductAsync(productDto);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Products.AddAsync(productDto.ToEntity()), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product1", Price = 10 };

            // Act
            await _productService.UpdateProductAsync(productDto);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Products.UpdateAsync(productDto.ToEntity()), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };

            _unitOfWorkMock.Setup(uow => uow.Products.GetByIdAsync(product.Id)).ReturnsAsync(product);
            _unitOfWorkMock.Setup(uow => uow.Products.DeleteAsync(product.Id)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(product.Id);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Products.GetByIdAsync(product.Id), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Products.DeleteAsync(product.Id), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}
