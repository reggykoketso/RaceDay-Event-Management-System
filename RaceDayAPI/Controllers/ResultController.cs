using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var results = await _context.Results.ToListAsync();
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> CreateResult(Result newResult)
        {
            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResults),
                new { id = newResult.ResultID },
                newResult);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, Result updatedResult)
        {
            var existingResult = await _context.Results.FindAsync(id);

            if (existingResult == null)
                return NotFound();

            existingResult.EnrolmentID = updatedResult.EnrolmentID;
            existingResult.FinishTime = updatedResult.FinishTime;
            existingResult.FinishPosition = updatedResult.FinishPosition;

            await _context.SaveChangesAsync();

            return Ok(existingResult);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound();

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}