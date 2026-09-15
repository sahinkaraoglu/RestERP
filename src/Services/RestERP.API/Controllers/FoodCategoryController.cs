using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.FoodCategories.Commands.CreateFoodCategory;
using RestERP.Application.Features.FoodCategories.Commands.DeleteFoodCategory;
using RestERP.Application.Features.FoodCategories.Commands.UpdateFoodCategory;
using RestERP.Application.Features.FoodCategories.Queries.GetFoodCategories;
using RestERP.Application.Features.FoodCategories.Queries.GetFoodCategoryById;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodCategoryController : BaseApiController
    {
        private readonly ILogger<FoodCategoryController> _logger;

        public FoodCategoryController(ILogger<FoodCategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodCategory>>> GetAllFoodCategories()
        {
            try
            {
                var categories = await Mediator.Send(new GetFoodCategoriesQuery());
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm yemek kategorileri getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodCategory>> GetFoodCategoryById(int id)
        {
            try
            {
                var category = await Mediator.Send(new GetFoodCategoryByIdQuery(id));
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori getirilirken hata oluştu: {CategoryId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPost]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<ActionResult<FoodCategory>> CreateFoodCategory([FromBody] FoodCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdCategory = await Mediator.Send(new CreateFoodCategoryCommand(category));
                return CreatedAtAction(nameof(GetFoodCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> UpdateFoodCategory(int id, [FromBody] FoodCategory category)
        {
            try
            {
                if (id != category.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await Mediator.Send(new UpdateFoodCategoryCommand(category));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori güncellenirken hata oluştu: {CategoryId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> DeleteFoodCategory(int id)
        {
            try
            {
                await Mediator.Send(new DeleteFoodCategoryCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek kategori bulunamadı: {CategoryId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori silinirken hata oluştu: {CategoryId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
