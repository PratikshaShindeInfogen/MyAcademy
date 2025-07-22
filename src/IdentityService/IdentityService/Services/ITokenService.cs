using IdentityService.Models;
using IdentityService.DTOs;

namespace IdentityService.Services.Interfaces
{
    public interface ITokenService
    {
        Task<AuthResult> GenerateTokensAsync(AuthUser user);

        Task<AuthResult> RefreshTokenAsync(string token);


    }
}
