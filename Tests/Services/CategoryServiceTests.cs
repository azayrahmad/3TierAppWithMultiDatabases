using Data.Models.ProductDb;
using Moq;
using Services.DTOs;
using Services.Services;
using Services.UnitOfWork;

namespace Tests.Services.Services
{
    public class CategoryServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _categoryService = new CategoryService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategoriesAsDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _unitOfWorkMock.Setup(u => u.Categories.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Category 1"));
            Assert.That(result.Last().Id, Is.EqualTo(2));
            Assert.That(result.Last().Name, Is.EqualTo("Category 2"));
        }

        [Test]
        public async Task GetCategoryByIdAsync_ShouldReturnCategoryAsDto()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Category 1"));
        }

        [Test]
        public async Task AddCategoryAsync_ShouldAddCategoryToUnitOfWork()
        {
            // Arrange
            var categoryDto = new CategoryDto { Id = 1, Name = "Category 1" };
            _unitOfWorkMock.Setup(u => u.Categories.AddAsync(It.IsAny<Category>())).Verifiable();
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await _categoryService.AddCategoryAsync(categoryDto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Categories.AddAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_ShouldUpdateCategoryInUnitOfWork()
        {
            // Arrange
            var categoryDto = new CategoryDto { Id = 1, Name = "Category 1" };
            _unitOfWorkMock.Setup(u => u.Categories.UpdateAsync(It.IsAny<Category>())).Verifiable();
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await _categoryService.UpdateCategoryAsync(categoryDto);

            // Assert
            _unitOfWorkMock.Verify(u => u.Categories.UpdateAsync(It.IsAny<Category>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_ShouldDeleteCategoryFromUnitOfWork()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Categories.DeleteAsync(1)).Verifiable();
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await _categoryService.DeleteCategoryAsync(1);

            // Assert
            _unitOfWorkMock.Verify(u => u.Categories.DeleteAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}