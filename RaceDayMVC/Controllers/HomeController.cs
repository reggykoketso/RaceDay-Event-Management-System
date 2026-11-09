
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Diagnostics;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            try
            {
                var events = await client.GetFromJsonAsync<List<Event>>("api/Event");

                var upcomingEvents = events?
                    .Where(e => e.EventDate >= DateTime.Today)
                    .OrderBy(e => e.EventDate)
                    .ToList() ?? new List<Event>();

                return View(upcomingEvents);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Could not retrieve events from RaceDay API.");

                ViewBag.ErrorMessage = "Unable to load events right now. Please try again later.";

                return View(new List<Event>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
