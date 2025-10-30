using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
using RestERP.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace RestERP.Application.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly RestERPDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AuthService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            RestERPDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var dbUser = await _userManager.FindByEmailAsync(request.Email);

                if (dbUser == null || !dbUser.IsActive)
                {
                    _logger.LogWarning($"Login başarısız - Kullanıcı bulunamadı/aktif değil: {request.Email}");
                    return null;
                }

                var passwordCheck = await _signInManager.CheckPasswordSignInAsync(dbUser, request.Password, lockoutOnFailure: false);
                if (!passwordCheck.Succeeded)
                {
                    _logger.LogWarning($"Login başarısız - Geçersiz şifre: {request.Email}");
                    return null;
                }

                var tokenResponse = await GenerateTokensAsync(dbUser);
                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login işlemi sırasında hata oluştu: {request.Email}");
                throw;
            }
        }

        public async Task<TokenResponse?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Email ve kullanıcı adı kontrolü
                var existingByEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingByEmail != null)
                {
                    _logger.LogWarning($"Kayıt başarısız - Email zaten kullanılıyor: {request.Email}");
                    return null;
                }

                var existingByName = await _userManager.FindByNameAsync(request.UserName);
                if (existingByName != null)
                {
                    _logger.LogWarning($"Kayıt başarısız - Kullanıcı adı zaten kullanılıyor: {request.UserName}");
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
                    _logger.LogWarning($"Kayıt başarısız - {errors}");
                    return null;
                }

                // Rol ataması (enum string adı ile)
                var roleName = user.RoleType.ToString();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);

                _logger.LogInformation($"Yeni kullanıcı kaydedildi: {user.Email}");

                var tokenResponse = await GenerateTokensAsync(user);
                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kayıt işlemi sırasında hata oluştu: {request.Email}");
                throw;
            }
        }

        public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var token = await _context.RefreshTokens
                    .Include(rt => rt.User)
                    .FirstOrDefaultAsync(rt => 
                        rt.Token == refreshToken && 
                        !rt.IsRevoked && 
                        rt.ExpiresAt > DateTime.UtcNow &&
                        rt.User.IsActive);

                if (token == null)
                {
                    _logger.LogWarning("Geçersiz veya süresi dolmuş refresh token");
                    return null;
                }

                // Eski refresh token'ı iptal et
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Yeni tokenlar oluştur
                var tokenResponse = await GenerateTokensAsync(token.User);
                return tokenResponse;
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
                var token = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => 
                        rt.Token == refreshToken && 
                        !rt.IsRevoked &&
                        rt.ExpiresAt > DateTime.UtcNow);

                if (token == null)
                {
                    return false;
                }

                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Refresh token iptal edildi: {refreshToken.Substring(0, Math.Min(10, refreshToken.Length))}...");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token iptal işlemi sırasında hata oluştu");
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<TokenResponse> GenerateTokensAsync(ApplicationUser user)
        {
            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30), // Refresh token 30 gün geçerli
                CreatedDate = DateTime.UtcNow
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"] ?? "7")),
                RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.RoleType.ToString()
                }
            };
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT anahtarı yapılandırılmamış.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleType.ToString())
            };

            // Identity rollerini de ekle
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"] ?? "7"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

