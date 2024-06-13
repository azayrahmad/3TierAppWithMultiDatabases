using Services.DTOs;
using Services.UnitOfWork;

namespace Services.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Select(p => p.ToDto());
        }
        public async Task<ProductDto> GetProductByIdWithTransactionsAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            var transactions = await _unitOfWork.Transactions.GetAllByProductIdAsync(id);
            var users = await _unitOfWork.Users.GetAllAsync();
            var transactionDtos = transactions.Select(t =>
            {
                var user = users.FirstOrDefault(u => u.Id == t.UserId);
                return t.ToDto(user, product);
            });

            var productDto = product.ToDto();
            productDto.Transactions = transactionDtos;
            return productDto;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product.ToDto();
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            await _unitOfWork.Products.AddAsync(productDto.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            await _unitOfWork.Products.UpdateAsync(productDto.ToEntity());
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
