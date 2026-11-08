using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventController(RaceDayContext context)
        {
            _context = context;
        }

        // Participants and organisers can view all events.
        [Authorize(Roles = "Participant,Organiser")]
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // Participants and organisers can view one event.
        [Authorize(Roles = "Participant,Organiser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
                return NotFound();

            return Ok(existingEvent);
        }

        // Only organisers can create events.
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = newEvent.EventID },
                newEvent);
        }

        // Only organisers can update events.
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
                return NotFound();

            existingEvent.OrganiserId = updatedEvent.OrganiserId;
            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Distance = updatedEvent.Distance;
            existingEvent.EventType = updatedEvent.EventType;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        // Only organisers can delete events.
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
                return NotFound();

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}