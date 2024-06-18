using Data.Models.ProductDb;
using System.ComponentModel.DataAnnotations;

namespace Services.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<Product>? Products { get; set; } = new List<Product>();
    }
}
