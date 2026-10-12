using Microsoft.AspNetCore.Mvc;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEvents()
        {
            var events = new[]
            {
                new { Id = 1, Name = "Polokwane Marathon", Location = "Polokwane" },
                new { Id = 2, Name = "RaceDay Fun Run", Location = "Limpopo" }
            };

            return Ok(events);
        }
    }
}