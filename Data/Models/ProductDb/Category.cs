using System.ComponentModel.DataAnnotations;

namespace Data.Models.ProductDb
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        public List<Product>? Products { get; set; }

    }
}
