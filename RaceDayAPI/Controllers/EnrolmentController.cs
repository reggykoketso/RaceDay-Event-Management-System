using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrolments()
        {
            var enrolments = await _context.Enrolments.ToListAsync();
            return Ok(enrolments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(Enrolment newEnrolment)
        {
            _context.Enrolments.Add(newEnrolment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolments),
                new { id = newEnrolment.EnrolmentID },
                newEnrolment);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrolment(
    int id, Enrolment updatedEnrolment)
        {
            var existingEnrolment = await _context.Enrolments.FindAsync(id);

            if (existingEnrolment == null)
                return NotFound();

            existingEnrolment.ParticipantID = updatedEnrolment.ParticipantID;
            existingEnrolment.EventID = updatedEnrolment.EventID;
            existingEnrolment.CategoryID = updatedEnrolment.CategoryID;
            existingEnrolment.EnrolmentDate = updatedEnrolment.EnrolmentDate;

            await _context.SaveChangesAsync();

            return Ok(existingEnrolment);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment = await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
                return NotFound();

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}