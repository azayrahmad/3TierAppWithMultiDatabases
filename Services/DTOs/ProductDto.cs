﻿using Data.Models.ProductDb;

namespace Services.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        public IEnumerable<TransactionDto>? Transactions { get; set; }
    }
}