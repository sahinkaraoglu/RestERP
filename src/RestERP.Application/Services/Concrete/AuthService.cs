using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RestERP.Application.DTOs;
using RestERP.Application.Features.Auth;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;

namespace RestERP.Application.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthTokenService _authTokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            IRefreshTokenRepository refreshTokenRepository,
            AuthTokenService authTokenService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _refreshTokenRepository = refreshTokenRepository;
            _authTokenService = authTokenService;
            _logger = logger;
        }

        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var dbUser = await _userManager.FindByEmailAsync(request.Email);
                if (dbUser == null || !dbUser.IsActive)
                {
                    _logger.LogWarning("Login başarısız - Kullanıcı bulunamadı/aktif değil: {Email}", request.Email);
                    return null;
                }

                var passwordCheck = await _signInManager.CheckPasswordSignInAsync(dbUser, request.Password, lockoutOnFailure: false);
                if (!passwordCheck.Succeeded)
                {
                    _logger.LogWarning("Login başarısız - Geçersiz şifre: {Email}", request.Email);
                    return null;
                }

                return await _authTokenService.GenerateTokensAsync(dbUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında hata oluştu: {Email}", request.Email);
                throw;
            }
        }

        public async Task<TokenResponse?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingByEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingByEmail != null)
                {
                    _logger.LogWarning("Kayıt başarısız - Email zaten kullanılıyor: {Email}", request.Email);
                    return null;
                }

                var existingByName = await _userManager.FindByNameAsync(request.UserName);
                if (existingByName != null)
                {
                    _logger.LogWarning("Kayıt başarısız - Kullanıcı adı zaten kullanılıyor: {UserName}", request.UserName);
                    return null;
                }

                var user = new ApplicationUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    IsActive = true,
                    RoleType = RestERP.Domain.Enums.Role.Customer
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Kayıt başarısız - {Errors}", errors);
                    return null;
                }

                var roleName = user.RoleType.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);

                _logger.LogInformation("Yeni kullanıcı kaydedildi: {Email}", user.Email);
                return await _authTokenService.GenerateTokensAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu: {Email}", request.Email);
                throw;
            }
        }

        public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var token = await _refreshTokenRepository.GetFirstOrDefaultAsync(
                    rt => rt.Token == refreshToken &&
                          !rt.IsRevoked &&
                          rt.ExpiresAt > DateTime.UtcNow,
                    "User");

                if (token == null || token.User == null || !token.User.IsActive)
                {
                    _logger.LogWarning("Geçersiz veya süresi dolmuş refresh token");
                    return null;
                }

                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                _refreshTokenRepository.Update(token);
                await _refreshTokenRepository.SaveChangesAsync();

                return await _authTokenService.GenerateTokensAsync(token.User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                throw;
            }
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var token = await _refreshTokenRepository.GetFirstOrDefaultAsync(rt =>
                    rt.Token == refreshToken &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > DateTime.UtcNow);

                if (token == null)
                    return false;

                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                _refreshTokenRepository.Update(token);
                await _refreshTokenRepository.SaveChangesAsync();

                _logger.LogInformation("Refresh token iptal edildi: {TokenPrefix}...",
                    refreshToken.Substring(0, Math.Min(10, refreshToken.Length)));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token iptal işlemi sırasında hata oluştu");
                return false;
            }
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            return Task.FromResult(_authTokenService.ValidateToken(token));
        }
    }
}
