using Data.Models.ProductDb;
using Services.DTOs;
using Moq;
using Services;
using Services.UnitOfWork;
using Data.Contexts;
using Castle.Components.DictionaryAdapter.Xml;
using Services.Services;
using Data.Models.UserDb;

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
            Assert.That(result.Id, Is.EqualTo(product.Id));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public async Task AddProductAsync_AddsProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Product1", Price = 10 };
            _unitOfWorkMock.Setup(uow => uow.Products.GetByIdAsync(It.Is<int>(id => id == productDto.Id))).ReturnsAsync(productDto.ToEntity());


            // Act
            await _productService.AddProductAsync(productDto);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Products.AddAsync(It.Is<Product>(p => p.Id == productDto.Id)), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            var updatedProductDto = product.ToDto();
            updatedProductDto.Name = "Prduct1 Updated";
            updatedProductDto.Price = 20;
            _unitOfWorkMock.Setup(uow => uow.Products.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);


            // Act
            await _productService.UpdateProductAsync(updatedProductDto);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.Products.UpdateAsync(It.Is<Product>(p => p.Name == updatedProductDto.Name)), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product1", Price = 10 };
            _unitOfWorkMock.Setup(uow => uow.Products.DeleteAsync(It.IsAny<int>())).Verifiable();

            // Act
            await _productService.DeleteProductAsync(product.Id);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Products.DeleteAsync(It.Is<int>(id => id == product.Id)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }
    }
}
