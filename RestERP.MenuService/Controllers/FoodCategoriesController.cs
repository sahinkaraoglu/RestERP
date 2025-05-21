using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestERP.MenuService.Data;
using RestERP.MenuService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.MenuService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodCategoriesController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public FoodCategoriesController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/FoodCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodCategory>>> GetFoodCategories()
        {
            return await _context.FoodCategories.ToListAsync();
        }

        // GET: api/FoodCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodCategory>> GetFoodCategory(int id)
        {
            var category = await _context.FoodCategories
                .Include(c => c.Foods)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/FoodCategories
        [HttpPost]
        public async Task<ActionResult<FoodCategory>> PostFoodCategory(FoodCategory category)
        {
            _context.FoodCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFoodCategory), new { id = category.Id }, category);
        }

        // PUT: api/FoodCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodCategory(int id, FoodCategory category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/FoodCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodCategory(int id)
        {
            var category = await _context.FoodCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.FoodCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.FoodCategories.Any(e => e.Id == id);
        }
    }
} 