
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private bool IsOrganiser()
        {
            return !string.IsNullOrEmpty(
                       HttpContext.Session.GetString("UserID"))
                   && HttpContext.Session.GetString("UserRole") == "Organiser";
        }

        private HttpClient GetApiClient()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            var client = GetApiClient();

            var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                "api/Event") ?? new List<EventViewModel>();

            var enrolments = await client.GetFromJsonAsync<List<EnrolmentViewModel>>(
                "api/Enrolment") ?? new List<EnrolmentViewModel>();

            ViewBag.EventCount = events.Count;
            ViewBag.EnrolmentCount = enrolments.Count;

            return View();
        }

        // Display all events to the organiser.
        public async Task<IActionResult> Events()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                "api/Event") ?? new List<EventViewModel>();

            return View(events);
        }

        // Display all participant enrolments.
        public async Task<IActionResult> Enrolments()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var enrolments = await client.GetFromJsonAsync<List<EnrolmentViewModel>>(
                "api/Enrolment") ?? new List<EnrolmentViewModel>();

            var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                "api/Event") ?? new List<EventViewModel>();

            var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
                "api/Category") ?? new List<CategoryViewModel>();

            foreach (var enrolment in enrolments)
            {
                enrolment.EventName = events
                    .FirstOrDefault(e => e.EventID == enrolment.EventID)
                    ?.Name ?? "Event unavailable";

                enrolment.CategoryName = categories
                    .FirstOrDefault(c => c.CategoryID == enrolment.CategoryID)
                    ?.CategoryName ?? "Category unavailable";
            }

            return View(enrolments);
        }
    }
}
