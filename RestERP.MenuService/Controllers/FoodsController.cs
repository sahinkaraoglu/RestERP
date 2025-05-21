using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestERP.MenuService.Data;
using RestERP.MenuService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.MenuService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodsController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public FoodsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _context.Foods
                .Include(f => f.Category)
                .ToListAsync();
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // GET: api/Foods/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsByCategory(int categoryId)
        {
            return await _context.Foods
                .Where(f => f.CategoryId == categoryId)
                .Include(f => f.Category)
                .ToListAsync();
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFood), new { id = food.Id }, food);
        }

        // PUT: api/Foods/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int id, Food food)
        {
            if (id != food.Id)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Foods/5/availability
        [HttpPatch("{id}/availability")]
        public async Task<IActionResult> UpdateAvailability(int id, [FromBody] bool isAvailable)
        {
            var food = await _context.Foods.FindAsync(id);
            
            if (food == null)
            {
                return NotFound();
            }

            food.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
} 