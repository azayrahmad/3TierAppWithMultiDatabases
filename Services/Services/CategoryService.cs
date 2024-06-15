using Services.DTOs;
using Services.UnitOfWork;

namespace Services.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => c.ToDto());
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return category.ToDto();
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            await _unitOfWork.Categories.AddAsync(categoryDto.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            await _unitOfWork.Categories.UpdateAsync(categoryDto.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
