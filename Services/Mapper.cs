using Data.Models.ProductDb;
using Data.Models.UserDb;
using Data.Models.TransactionDb;
using Services.DTOs;

namespace Services
{
    public static class Mapper
    {
        public static TransactionDto ToDto(this Transaction transaction, User user, Product product)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                UserName = user.Name,
                ProductId = transaction.ProductId,
                ProductName = product.Name,
                TransactionDate = transaction.TransactionDate,
                Status = transaction.Status
            };
        }

        public static Transaction ToEntity(this TransactionDto transactionDto)
        {
            return new Transaction
            {
                Id = transactionDto.Id,
                UserId = transactionDto.UserId,
                ProductId = transactionDto.ProductId,
                TransactionDate = transactionDto.TransactionDate,
                Status = transactionDto.Status
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public static User ToEntity(this UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = userDto.Email
            };
        }

        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Category = product.Category, 
                Price = product.Price
            };
        }

        public static Product ToEntity(this ProductDto productDto)
        {
            return new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                CategoryId = productDto.CategoryId,
                Category = productDto.Category,
                Price = productDto.Price
            };
        }

        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products
            };
        }

        public static Category ToEntity(this CategoryDto categoryDto)
        {
            return new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Products = categoryDto.Products
            };
        }
    }
}
