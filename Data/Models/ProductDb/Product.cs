using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Models.ProductDb
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal Price { get; set; }
    }
}
