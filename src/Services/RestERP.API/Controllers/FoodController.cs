using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.FoodCategories.Queries.GetFoodCategories;
using RestERP.Application.Features.Foods.Commands.CreateFood;
using RestERP.Application.Features.Foods.Commands.DeleteFood;
using RestERP.Application.Features.Foods.Commands.UpdateFood;
using RestERP.Application.Features.Foods.Queries.GetFoodById;
using RestERP.Application.Features.Foods.Queries.GetFoodImages;
using RestERP.Application.Features.Foods.Queries.GetFoods;
using RestERP.Application.Features.Foods.Queries.GetFoodsByCategory;
using RestERP.Application.Features.Foods.Queries.GetFoodsBySubCategory;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : BaseApiController
    {
        private readonly ILogger<FoodController> _logger;

        public FoodController(ILogger<FoodController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetAllFoods()
        {
            try
            {
                var foods = await Mediator.Send(new GetFoodsQuery());
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm yemekler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFoodById(int id)
        {
            try
            {
                var food = await Mediator.Send(new GetFoodByIdQuery(id));
                return Ok(food);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek getirilirken hata oluştu: {FoodId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsByCategory(int categoryId)
        {
            try
            {
                var foods = await Mediator.Send(new GetFoodsByCategoryQuery(categoryId));
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriye göre yemekler getirilirken hata oluştu: {CategoryId}", categoryId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("subcategory/{subCategoryId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsBySubCategory(int subCategoryId)
        {
            try
            {
                var foods = await Mediator.Send(new GetFoodsBySubCategoryQuery(subCategoryId));
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt kategoriye göre yemekler getirilirken hata oluştu: {SubCategoryId}", subCategoryId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Food>> CreateFood([FromBody] Food food)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdFood = await Mediator.Send(new CreateFoodCommand(food));
                return CreatedAtAction(nameof(GetFoodById), new { id = createdFood.Id }, createdFood);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] Food food)
        {
            try
            {
                if (id != food.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await Mediator.Send(new UpdateFoodCommand(food));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek güncellenirken hata oluştu: {FoodId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteFood(int id)
        {
            try
            {
                await Mediator.Send(new DeleteFoodCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek yemek bulunamadı: {FoodId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek silinirken hata oluştu: {FoodId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<FoodCategory>>> GetAllFoodCategories()
        {
            try
            {
                var categories = await Mediator.Send(new GetFoodCategoriesQuery());
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek kategorileri getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("images")]
        public async Task<ActionResult<IEnumerable<Image>>> GetAllFoodImages()
        {
            try
            {
                var images = await Mediator.Send(new GetFoodImagesQuery());
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek resimleri getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
