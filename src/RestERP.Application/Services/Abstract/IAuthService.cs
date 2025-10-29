using RestERP.Application.DTOs;

namespace RestERP.Application.Services.Abstract
{
    public interface IAuthService
    {
        Task<TokenResponse?> LoginAsync(LoginRequest request);
        Task<TokenResponse?> RegisterAsync(RegisterRequest request);
        Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}

