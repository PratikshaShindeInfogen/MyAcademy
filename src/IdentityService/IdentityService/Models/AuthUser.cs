using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class AuthUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        // Navigation property to refresh tokens
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
