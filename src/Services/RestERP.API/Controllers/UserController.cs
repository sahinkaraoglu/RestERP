using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Features.Users.Commands.CreateUser;
using RestERP.Application.Features.Users.Commands.DeleteUser;
using RestERP.Application.Features.Users.Commands.ResetPassword;
using RestERP.Application.Features.Users.Commands.UpdateUser;
using RestERP.Application.Features.Users.Queries.GetUserByEmail;
using RestERP.Application.Features.Users.Queries.GetUserById;
using RestERP.Application.Features.Users.Queries.GetUserByUsername;
using RestERP.Application.Features.Users.Queries.GetUsers;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            try
            {
                var users = await Mediator.Send(new GetUsersQuery());
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm kullanıcılar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserById(int id)
        {
            try
            {
                var user = await Mediator.Send(new GetUserByIdQuery(id));
                if (user == null)
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");

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

        [HttpGet("username/{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserByUsername(string username)
        {
            try
            {
                var user = await Mediator.Send(new GetUserByUsernameQuery(username));
                if (user == null)
                    return NotFound($"Kullanıcı adı {username} olan kullanıcı bulunamadı");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı adı ile kullanıcı getirilirken hata oluştu: {Username}", username);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("email/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmail(string email)
        {
            try
            {
                var user = await Mediator.Send(new GetUserByEmailQuery(email));
                if (user == null)
                    return NotFound($"Email {email} olan kullanıcı bulunamadı");

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await Mediator.Send(new CreateUserCommand(user, string.Empty));
                if (createdUser.Succeeded)
                    return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

                return BadRequest("Kullanıcı oluşturulamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] ApplicationUser user)
        {
            try
            {
                if (id != user.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new UpdateUserCommand(user));
                if (!result)
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");

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

        [HttpPost("{id}/reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await Mediator.Send(new ResetPasswordCommand(id, request.NewPassword));
                if (!result.Succeeded)
                    return BadRequest(new { message = "Şifre sıfırlanamadı", errors = result.Errors });

                return Ok(new { message = "Şifre başarıyla sıfırlandı" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı şifresi sıfırlanırken hata oluştu: {UserId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteUserCommand(id));
                if (!result)
                    return NotFound($"ID {id} olan kullanıcı bulunamadı");

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

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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
