using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> CreateRoute(RaceDayAPI.Models.Route newRoute)
        {
            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoutes), new { id = newRoute.RouteID }, newRoute);
        }
    }
}