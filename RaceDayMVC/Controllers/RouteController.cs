using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class RouteController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public RouteController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient Api()
        {
            var client = _factory.CreateClient("RaceDayAPI");
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        private bool Organiser() =>
            HttpContext.Session.GetString("UserRole") == "Organiser";

        private bool LoggedIn() =>
            !string.IsNullOrEmpty(HttpContext.Session.GetString("UserID"));

        public async Task<IActionResult> Index()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Auth");

            var response = await Api().GetAsync("api/Route");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load routes.";
                return View(new List<RouteViewModel>());
            }

            var routes = await response.Content
                .ReadFromJsonAsync<List<RouteViewModel>>();

            return View(routes ?? new List<RouteViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteViewModel route)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(route);

            var response = await Api().PostAsJsonAsync("api/Route", route);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not create route.";
                return View(route);
            }

            TempData["Success"] = "Route created successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            var response = await Api().GetAsync("api/Route");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var routes = await response.Content
                .ReadFromJsonAsync<List<RouteViewModel>>();

            var route = routes?.FirstOrDefault(r => r.RouteID == id);

            if (route == null)
                return NotFound();

            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RouteViewModel route)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(route);

            route.RouteID = id;

            var response = await Api().PutAsJsonAsync(
                $"api/Route/{id}", route);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not update route.";
                return View(route);
            }

            TempData["Success"] = "Route updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            var response = await Api().DeleteAsync($"api/Route/{id}");

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode
                    ? "Route deleted successfully."
                    : "Could not delete route.";

            return RedirectToAction("Index");
        }
    }
}