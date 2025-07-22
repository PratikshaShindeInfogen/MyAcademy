using System.ComponentModel.DataAnnotations;

namespace IdentityService.DTOs
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
