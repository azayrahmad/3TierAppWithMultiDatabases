using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
