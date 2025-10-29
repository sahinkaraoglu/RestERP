using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodCategoryController : ControllerBase
    {
        private readonly IFoodCategoryService _foodCategoryService;
        private readonly ILogger<FoodCategoryController> _logger;

        public FoodCategoryController(IFoodCategoryService foodCategoryService, ILogger<FoodCategoryController> logger)
        {
            _foodCategoryService = foodCategoryService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm yemek kategorilerini getirir
        /// </summary>
        /// <returns>Kategori listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodCategory>>> GetAllFoodCategories()
        {
            try
            {
                var categories = await _foodCategoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm yemek kategorileri getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre yemek kategorisi getirir
        /// </summary>
        /// <param name="id">Kategori ID'si</param>
        /// <returns>Kategori bilgisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodCategory>> GetFoodCategoryById(int id)
        {
            try
            {
                var category = await _foodCategoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"ID {id} olan kategori bulunamadı");
                }
                return Ok(category);
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

        /// <summary>
        /// Yeni yemek kategorisi oluşturur
        /// </summary>
        /// <param name="category">Kategori bilgileri</param>
        /// <returns>Oluşturulan kategori</returns>
        [HttpPost]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<ActionResult<FoodCategory>> CreateFoodCategory([FromBody] FoodCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdCategory = await _foodCategoryService.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetFoodCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yemek kategorisi bilgilerini günceller
        /// </summary>
        /// <param name="id">Kategori ID'si</param>
        /// <param name="category">Güncellenecek kategori bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> UpdateFoodCategory(int id, [FromBody] FoodCategory category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _foodCategoryService.UpdateCategoryAsync(category);
                return NoContent();
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

        /// <summary>
        /// Yemek kategorisi siler
        /// </summary>
        /// <param name="id">Silinecek kategori ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> DeleteFoodCategory(int id)
        {
            try
            {
                await _foodCategoryService.DeleteCategoryAsync(id);
                return NoContent();
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
