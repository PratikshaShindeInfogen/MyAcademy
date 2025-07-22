using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Token { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string JwtId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool IsUsed { get; set; } = false;

        [Required]
        public bool IsRevoked { get; set; } = false;

        // Foreign Key to User
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AuthUser User { get; set; }
    }
}
