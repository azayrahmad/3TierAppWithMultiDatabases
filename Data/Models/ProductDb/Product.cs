using System.ComponentModel.DataAnnotations;

namespace Data.Models.ProductDb
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }
    }
}
