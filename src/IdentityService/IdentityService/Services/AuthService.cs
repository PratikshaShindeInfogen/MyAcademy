using IdentityService.Data;
using IdentityService.DTOs;
using IdentityService.Models;
using IdentityService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IdentityDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AuthService(
            UserManager<AuthUser> userManager,
            ITokenService tokenService,
            IdentityDbContext context,
            IOptions<JwtSettings> jwtOptions, 
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _jwtSettings = jwtOptions.Value;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResult { Success = false, Errors = new List<string> { "User already exists" } };
            }

            var newUser = new AuthUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            var client = _configuration["UserService:BaseUrl"];

            var user = new
            {
                Id = Guid.Parse(newUser.Id),
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                FullName = newUser.FullName,
            };
            var response = await _httpClient.PostAsJsonAsync($"{client}/v1/api/user", user);
            if (!response.IsSuccessStatusCode)
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = new List<string> { "Failed to create user in User Service" }
                };
            }
            return await _tokenService.GenerateTokensAsync(newUser);
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResult { Success = false, Errors = new List<string> { "Invalid login" } };
            }

            return await _tokenService.GenerateTokensAsync(user);
        }

        public async Task<AuthResult> RefreshTokenAsync(string token)
        {
            return await _tokenService.RefreshTokenAsync(token);
        }
    }
}
