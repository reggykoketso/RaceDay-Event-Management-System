
using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateApiClient()
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

        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(
                HttpContext.Session.GetString("UserID"));
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("UserRole") == "Organiser";
        }

        // Display all events publicly.
        public async Task<IActionResult> Index()
        {
            var client = CreateApiClient();

            var response = await client.GetAsync("api/Event");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load events.";
                return View(new List<EventViewModel>());
            }

            var events = await response.Content
                .ReadFromJsonAsync<List<EventViewModel>>();

            return View(events ?? new List<EventViewModel>());
        }

        // Display one event publicly.
        public async Task<IActionResult> Details(int id)
        {
            var client = CreateApiClient();

            var response = await client.GetAsync($"api/Event/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var eventItem = await response.Content
                .ReadFromJsonAsync<EventViewModel>();

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // Display create form.
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // Save new event.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventItem)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(eventItem);

            eventItem.OrganiserId = int.Parse(
                HttpContext.Session.GetString("UserID")!);

            var client = CreateApiClient();

            var response = await client.PostAsJsonAsync(
                "api/Event", eventItem);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not create the event.";
                return View(eventItem);
            }

            TempData["Success"] = "Event created successfully.";

            return RedirectToAction("Index");
        }

        // Display edit form.
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = CreateApiClient();

            var response = await client.GetAsync($"api/Event/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var eventItem = await response.Content
                .ReadFromJsonAsync<EventViewModel>();

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // Save event changes.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, EventViewModel eventItem)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(eventItem);

            eventItem.EventID = id;

            var client = CreateApiClient();

            var response = await client.PutAsJsonAsync(
                $"api/Event/{id}", eventItem);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not update the event.";
                return View(eventItem);
            }

            TempData["Success"] = "Event updated successfully.";

            return RedirectToAction("Index");
        }

        // Delete an event.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = CreateApiClient();

            var response = await client.DeleteAsync($"api/Event/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Event deleted successfully.";
            else
                TempData["Error"] = "Could not delete the event.";

            return RedirectToAction("Index");
        }
    
    
        // Upload an event banner through the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadBanner(IFormFile file)
        {
            if (!IsOrganiser())
                return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest("Please select an image.");

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("Image must be smaller than 5 MB.");

            var allowedTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Only JPG, PNG and WEBP images are allowed.");

            var client = CreateApiClient();

            using var form = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            form.Add(fileContent, "file", Path.GetFileName(file.FileName));

            var response = await client.PostAsync(
                "api/BlobStorage/upload", form);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

            using var result = await response.Content.ReadFromJsonAsync<JsonDocument>();

            if (result == null ||
                !result.RootElement.TryGetProperty("imageUrl", out var url))
            {
                return BadRequest("The API did not return an image URL.");
            }

            return Json(new { imageUrl = url.GetString() });
        }
    }
}


    
