using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner
        /// </summary>
        /// <param name="request">Giriş bilgileri (Email ve Password)</param>
        /// <returns>JWT Access Token ve Refresh Token</returns>
        /// <response code="200">Giriş başarılı</response>
        /// <response code="400">Geçersiz istek veya hatalı giriş bilgileri</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(request);
                
                if (result == null)
                {
                    return BadRequest(new { message = "Geçersiz email veya şifre" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur ve JWT token döner
        /// </summary>
        /// <param name="request">Kayıt bilgileri</param>
        /// <returns>JWT Access Token ve Refresh Token</returns>
        /// <response code="201">Kayıt başarılı</response>
        /// <response code="400">Geçersiz istek veya email/kullanıcı adı zaten kullanılıyor</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(request);
                
                if (result == null)
                {
                    return BadRequest(new { message = "Email veya kullanıcı adı zaten kullanılıyor" });
                }

                return CreatedAtAction(nameof(Login), new { }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Refresh token kullanarak yeni access token ve refresh token alır
        /// </summary>
        /// <param name="request">Refresh token</param>
        /// <returns>Yeni JWT Access Token ve Refresh Token</returns>
        /// <response code="200">Token yenileme başarılı</response>
        /// <response code="400">Geçersiz refresh token</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RefreshTokenAsync(request.RefreshToken);
                
                if (result == null)
                {
                    return BadRequest(new { message = "Geçersiz veya süresi dolmuş refresh token" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// Refresh token'ı iptal eder (logout işlemi için)
        /// </summary>
        /// <param name="request">İptal edilecek refresh token</param>
        /// <returns>İptal sonucu</returns>
        /// <response code="200">Token başarıyla iptal edildi</response>
        /// <response code="400">Geçersiz refresh token</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("revoke-token")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
                
                if (!result)
                {
                    return BadRequest(new { message = "Geçersiz refresh token" });
                }

                return Ok(new { message = "Token başarıyla iptal edildi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token iptal işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        /// <summary>
        /// JWT token'ın geçerliliğini kontrol eder
        /// </summary>
        /// <param name="token">Kontrol edilecek JWT token</param>
        /// <returns>Token geçerliliği</returns>
        /// <response code="200">Token geçerli</response>
        /// <response code="400">Token geçersiz</response>
        /// <response code="500">Sunucu hatası</response>
        [HttpPost("validate-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ValidateToken([FromBody] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token boş olamaz" });
                }

                var isValid = await _authService.ValidateTokenAsync(token);
                
                if (!isValid)
                {
                    return BadRequest(new { message = "Token geçersiz", isValid = false });
                }

                return Ok(new { message = "Token geçerli", isValid = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token doğrulama işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }
    }
}

