using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Services.Abstract;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/hc")]
    [AllowAnonymous]
    public class HealthCheckController : BaseApiController
    {

        public HealthCheckController()
        {
        }

      
        [HttpGet("")]
        public async Task<IActionResult> Hc()
        {

            return Ok(new {
                StatusCode = 200,
            });
        }
    }
}

