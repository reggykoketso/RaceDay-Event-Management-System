using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class EnrolmentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EnrolmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

        private bool IsParticipant()
        {
            return HttpContext.Session.GetString("UserRole") == "Participant"
                && !string.IsNullOrEmpty(
                    HttpContext.Session.GetString("UserID"));
        }

        // GET: Enrolment
        public async Task<IActionResult> Index()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = GetApiClient();

            var enrolments = await client.GetFromJsonAsync
                <List<EnrolmentViewModel>>("api/Enrolment")
                ?? new List<EnrolmentViewModel>();

            var events = await client.GetFromJsonAsync
                <List<EventViewModel>>("api/Event")
                ?? new List<EventViewModel>();

            var categories = await client.GetFromJsonAsync
                <List<CategoryViewModel>>("api/Category")
                ?? new List<CategoryViewModel>();

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

        // GET: Enrolment/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = GetApiClient();

            var model = new EnrolmentCreateViewModel
            {
                Events = await client.GetFromJsonAsync
                    <List<EventViewModel>>("api/Event")
                    ?? new List<EventViewModel>(),

                Categories = await client.GetFromJsonAsync
                    <List<CategoryViewModel>>("api/Category")
                    ?? new List<CategoryViewModel>()
            };

            return View(model);
        }

        // POST: Enrolment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            EnrolmentCreateViewModel model)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = GetApiClient();

            model.Events = await client.GetFromJsonAsync
                <List<EventViewModel>>("api/Event")
                ?? new List<EventViewModel>();

            model.Categories = await client.GetFromJsonAsync
                <List<CategoryViewModel>>("api/Category")
                ?? new List<CategoryViewModel>();

            var selectedCategory = model.Categories
                .FirstOrDefault(c => c.CategoryID == model.CategoryID);

            if (!model.Events.Any(e => e.EventID == model.EventID) ||
                selectedCategory == null ||
                selectedCategory.EventID != model.EventID)
            {
                ModelState.AddModelError("",
                    "Please select a valid event and category.");

                return View(model);
            }

            var enrolment = new
            {
                EventID = model.EventID,
                CategoryID = model.CategoryID
            };

            var response = await client.PostAsJsonAsync(
                "api/Enrolment", enrolment);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "You have successfully enrolled in the event.";

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("",
                "Your enrolment could not be completed. Please try again.");

            return View(model);
        }

        // POST: Enrolment/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = GetApiClient();

            var response = await client.DeleteAsync(
                $"api/Enrolment/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Your enrolment has been cancelled.";
            }
            else
            {
                TempData["Error"] =
                    "The enrolment could not be cancelled.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}