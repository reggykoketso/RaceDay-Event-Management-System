using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Organiser")]
    public class RouteController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public RouteController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            var routes = await _context.Routes.ToListAsync();
            return Ok(routes);
        }
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> CreateRoute(RaceDayAPI.Models.Route newRoute)
        {
            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoutes), new { id = newRoute.RouteID }, newRoute);
        }
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(
    int id,
    RaceDayAPI.Models.Route updatedRoute)
        {
            var existingRoute = await _context.Routes.FindAsync(id);

            if (existingRoute == null)
                return NotFound();

            existingRoute.EventID = updatedRoute.EventID;
            existingRoute.RouteName = updatedRoute.RouteName;
            existingRoute.StartLocation = updatedRoute.StartLocation;
            existingRoute.FinishLocation = updatedRoute.FinishLocation;
            existingRoute.Distance = updatedRoute.Distance;
            existingRoute.RouteDescription = updatedRoute.RouteDescription;

            await _context.SaveChangesAsync();

            return Ok(existingRoute);
        }
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
                return NotFound();

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}