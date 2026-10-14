using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public CategoryController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category newCategory)
        {
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new { id = newCategory.CategoryID }, newCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category updatedCategory)
        {
            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
                return NotFound();

            existingCategory.EventID = updatedCategory.EventID;
            existingCategory.CategoryName = updatedCategory.CategoryName;
            existingCategory.AgeMinimum = updatedCategory.AgeMinimum;
            existingCategory.AgeMaximum = updatedCategory.AgeMaximum;
            existingCategory.Distance = updatedCategory.Distance;

            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}