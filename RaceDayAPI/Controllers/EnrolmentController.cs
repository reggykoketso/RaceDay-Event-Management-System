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
    }
}