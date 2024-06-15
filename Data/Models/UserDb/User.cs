using System.ComponentModel.DataAnnotations;

namespace Data.Models.UserDb
{
    /// <summary>
    /// Store the user data
    /// </summary>
    public class User : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
