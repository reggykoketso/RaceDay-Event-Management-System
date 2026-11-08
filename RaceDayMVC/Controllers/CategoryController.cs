using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public CategoryController(IHttpClientFactory factory)
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

            var response = await Api().GetAsync("api/Category");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load categories.";
                return View(new List<CategoryViewModel>());
            }

            var categories = await response.Content
                .ReadFromJsonAsync<List<CategoryViewModel>>();

            return View(categories ?? new List<CategoryViewModel>());
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
        public async Task<IActionResult> Create(CategoryViewModel category)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(category);

            var response = await Api().PostAsJsonAsync("api/Category", category);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not create category.";
                return View(category);
            }

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            var response = await Api().GetAsync("api/Category");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var categories = await response.Content
                .ReadFromJsonAsync<List<CategoryViewModel>>();

            var category = categories?.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel category)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(category);

            category.CategoryID = id;

            var response = await Api().PutAsJsonAsync(
                $"api/Category/{id}", category);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not update category.";
                return View(category);
            }

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Organiser())
                return RedirectToAction("Login", "Auth");

            var response = await Api().DeleteAsync($"api/Category/{id}");

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode
                    ? "Category deleted successfully."
                    : "Could not delete category.";

            return RedirectToAction("Index");
        }
    }
}