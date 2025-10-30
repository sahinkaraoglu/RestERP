using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm kullanıcıları getirir
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm kullanıcılar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre kullanıcı getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı bilgisi</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");
                }
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Kullanıcı bulunamadı: {UserId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu: {UserId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Email'e göre kullanıcı getirir
        /// </summary>
        /// <param name="email">Email adresi</param>
        /// <returns>Kullanıcı bilgisi</returns>
        [HttpGet("email/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"Email {email} olan kullanıcı bulunamadı");
                }
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Email ile kullanıcı bulunamadı: {Email}", email);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email ile kullanıcı getirilirken hata oluştu: {Email}", email);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur
        /// </summary>
        /// <param name="user">Kullanıcı bilgileri</param>
        /// <returns>Oluşturulan kullanıcı</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdUser = await _userService.CreateUserAsync(user);
                if (createdUser)
                {
                    return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
                }
                return BadRequest("Kullanıcı oluşturulamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="user">Güncellenecek kullanıcı bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] ApplicationUser user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateUserAsync(user);
                if (!result)
                {
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");
                }
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek kullanıcı bulunamadı: {UserId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu: {UserId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Kullanıcı siler
        /// </summary>
        /// <param name="id">Silinecek kullanıcı ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");
                }
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek kullanıcı bulunamadı: {UserId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu: {UserId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        /// <param name="loginRequest">Giriş bilgileri</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Bu metod IUserService'de mevcut değil, şimdilik NotImplementedException fırlatıyoruz
                return StatusCode(501, "Login metodu henüz implement edilmedi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı girişi yapılırken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
