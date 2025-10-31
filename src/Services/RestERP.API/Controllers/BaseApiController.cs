using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Standart API response formatını döndürür
        /// </summary>
        /// <typeparam name="T">Response data tipi</typeparam>
        /// <param name="response">Response objesi</param>
        /// <returns>HTTP response</returns>
        protected IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        /// <summary>
        /// Başarılı response döndürür
        /// </summary>
        /// <typeparam name="T">Data tipi</typeparam>
        /// <param name="data">Response data</param>
        /// <param name="statusCode">HTTP status code (varsayılan: 200)</param>
        /// <returns>HTTP response</returns>
        protected IActionResult Success<T>(T data, int statusCode = 200) where T : class
        {
            return ActionResultInstance(Response<T>.Success(data, statusCode));
        }

        /// <summary>
        /// Başarılı response döndürür (data olmadan)
        /// </summary>
        /// <param name="statusCode">HTTP status code (varsayılan: 200)</param>
        /// <returns>HTTP response</returns>
        protected IActionResult Success(int statusCode = 200)
        {
            return ActionResultInstance(Response<NoDataDto>.Success(statusCode));
        }

        /// <summary>
        /// Hata response döndürür
        /// </summary>
        /// <typeparam name="T">Data tipi</typeparam>
        /// <param name="errorMessage">Hata mesajı</param>
        /// <param name="statusCode">HTTP status code (varsayılan: 400)</param>
        /// <param name="isShow">Hata kullanıcıya gösterilsin mi</param>
        /// <returns>HTTP response</returns>
        protected IActionResult Error<T>(string errorMessage, int statusCode = 400, bool isShow = true) where T : class
        {
            return ActionResultInstance(Response<T>.Fail(errorMessage, statusCode, isShow));
        }
    }
}