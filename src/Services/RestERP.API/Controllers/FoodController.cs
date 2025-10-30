using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly ILogger<FoodController> _logger;

        public FoodController(IFoodService foodService, ILogger<FoodController> logger)
        {
            _foodService = foodService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm yemekleri getirir
        /// </summary>
        /// <returns>Yemek listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetAllFoods()
        {
            try
            {
                var foods = await _foodService.GetAllFoodsAsync();
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm yemekler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre yemek getirir
        /// </summary>
        /// <param name="id">Yemek ID'si</param>
        /// <returns>Yemek bilgisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFoodById(int id)
        {
            try
            {
                var food = await _foodService.GetFoodByIdAsync(id);
                if (food == null)
                {
                    return NotFound($"ID {id} olan yemek bulunamadı");
                }
                return Ok(food);
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

        /// <summary>
        /// Kategoriye göre yemekleri getirir
        /// </summary>
        /// <param name="categoryId">Kategori ID'si</param>
        /// <returns>Kategoriye ait yemek listesi</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsByCategory(int categoryId)
        {
            try
            {
                var foods = await _foodService.GetFoodsByCategoryAsync(categoryId);
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriye göre yemekler getirilirken hata oluştu: {CategoryId}", categoryId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Alt kategoriye göre yemekleri getirir
        /// </summary>
        /// <param name="subCategoryId">Alt kategori ID'si</param>
        /// <returns>Alt kategoriye ait yemek listesi</returns>
        [HttpGet("subcategory/{subCategoryId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsBySubCategory(int subCategoryId)
        {
            try
            {
                var foods = await _foodService.GetFoodsBySubCategoryAsync(subCategoryId);
                return Ok(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt kategoriye göre yemekler getirilirken hata oluştu: {SubCategoryId}", subCategoryId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yeni yemek oluşturur
        /// </summary>
        /// <param name="food">Yemek bilgileri</param>
        /// <returns>Oluşturulan yemek</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Food>> CreateFood([FromBody] Food food)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdFood = await _foodService.CreateFoodAsync(food);
                return CreatedAtAction(nameof(GetFoodById), new { id = createdFood.Id }, createdFood);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yemek bilgilerini günceller
        /// </summary>
        /// <param name="id">Yemek ID'si</param>
        /// <param name="food">Güncellenecek yemek bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] Food food)
        {
            try
            {
                if (id != food.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _foodService.UpdateFoodAsync(food);
                return NoContent();
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

        /// <summary>
        /// Yemek siler
        /// </summary>
        /// <param name="id">Silinecek yemek ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteFood(int id)
        {
            try
            {
                await _foodService.DeleteFoodAsync(id);
                return NoContent();
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

        /// <summary>
        /// Tüm yemek kategorilerini getirir
        /// </summary>
        /// <returns>Kategori listesi</returns>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<FoodCategory>>> GetAllFoodCategories()
        {
            try
            {
                var categories = await _foodService.GetAllFoodCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yemek kategorileri getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Tüm yemek resimlerini getirir
        /// </summary>
        /// <returns>Resim listesi</returns>
        [HttpGet("images")]
        public async Task<ActionResult<IEnumerable<Image>>> GetAllFoodImages()
        {
            try
            {
                var images = await _foodService.GetAllFoodImagesAsync();
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
