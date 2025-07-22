using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }    

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
