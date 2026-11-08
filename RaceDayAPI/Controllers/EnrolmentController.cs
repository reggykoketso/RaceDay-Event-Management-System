
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
    public class EnrolmentController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentController(RaceDayContext context)
        {
            _context = context;
        }

        // Organisers can view all enrolments.
        // Participants can only view their own.
        [Authorize(Roles = "Participant,Organiser")]
        [HttpGet]
        public async Task<IActionResult> GetEnrolments()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (User.IsInRole("Organiser"))
            {
                var allEnrolments = await _context.Enrolments
                    .ToListAsync();

                return Ok(allEnrolments);
            }

            var participantId = int.Parse(userId);

            var myEnrolments = await _context.Enrolments
                .Where(e => e.ParticipantID == participantId)
                .ToListAsync();

            return Ok(myEnrolments);
        }

        // Participants can register for an event.
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(
            Enrolment newEnrolment)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int participantId))
                return Unauthorized();

            newEnrolment.ParticipantID = participantId;
            newEnrolment.EnrolmentDate = DateTime.Now;

            _context.Enrolments.Add(newEnrolment);

            await _context.SaveChangesAsync();

            return Ok(newEnrolment);
        }

        // Organisers can update enrolments.
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrolment(
            int id, Enrolment updatedEnrolment)
        {
            var existingEnrolment =
                await _context.Enrolments.FindAsync(id);

            if (existingEnrolment == null)
                return NotFound();

            existingEnrolment.ParticipantID =
                updatedEnrolment.ParticipantID;

            existingEnrolment.EventID =
                updatedEnrolment.EventID;

            existingEnrolment.CategoryID =
                updatedEnrolment.CategoryID;

            existingEnrolment.EnrolmentDate =
                updatedEnrolment.EnrolmentDate;

            await _context.SaveChangesAsync();

            return Ok(existingEnrolment);
        }

        // Participants can cancel their own enrolments.
        // Organisers can cancel any enrolment.
        [Authorize(Roles = "Participant,Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment =
                await _context.Enrolments.FindAsync(id);

            if (enrolment == null)
                return NotFound();

            if (User.IsInRole("Participant"))
            {
                var userId = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (!int.TryParse(userId, out int participantId))
                    return Unauthorized();

                if (enrolment.ParticipantID != participantId)
                    return Forbid();
            }

            _context.Enrolments.Remove(enrolment);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
