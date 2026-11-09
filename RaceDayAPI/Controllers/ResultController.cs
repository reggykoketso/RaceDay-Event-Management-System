
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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

        // Participants see only their own results.
        // Organisers can see all results.
        [Authorize(Roles = "Participant,Organiser")]
        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var results = _context.Results.AsQueryable();

            if (User.IsInRole("Participant"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userId, out int participantId))
                    return Unauthorized();

                results = results.Where(r =>
                    _context.Enrolments.Any(e =>
                        e.EnrolmentID == r.EnrolmentID &&
                        e.ParticipantID == participantId));
            }

            return Ok(await results.ToListAsync());
        }

        // Only organisers can add results.
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> CreateResult(Result newResult)
        {
            var enrolment = await _context.Enrolments
                .FindAsync(newResult.EnrolmentID);

            if (enrolment == null)
                return BadRequest("The enrolment does not exist.");

            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();

            return Ok(newResult);
        }

        // Only organisers can update results.
        [Authorize(Roles = "Organiser")]
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

        // Only organisers can delete results.
        [Authorize(Roles = "Organiser")]
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
