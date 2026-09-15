using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Features.Auth.Commands.Login;
using RestERP.Application.Features.Auth.Commands.RefreshToken;
using RestERP.Application.Features.Auth.Commands.Register;
using RestERP.Application.Features.Auth.Commands.RevokeRefreshToken;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new LoginCommand(request));
                if (result == null)
                    return BadRequest(new { message = "Geçersiz email veya şifre" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new RegisterCommand(request));
                if (result == null)
                    return BadRequest(new { message = "Email veya kullanıcı adı zaten kullanılıyor" });

                return CreatedAtAction(nameof(Login), new { }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new RefreshTokenCommand(request.RefreshToken));
                if (result == null)
                    return BadRequest(new { message = "Geçersiz veya süresi dolmuş refresh token" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }

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
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new RevokeRefreshTokenCommand(request.RefreshToken));
                if (!result)
                    return BadRequest(new { message = "Geçersiz refresh token" });

                return Ok(new { message = "Token başarıyla iptal edildi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token iptal işlemi sırasında hata oluştu");
                return StatusCode(500, new { message = "Sunucu hatası oluştu" });
            }
        }
    }
}
