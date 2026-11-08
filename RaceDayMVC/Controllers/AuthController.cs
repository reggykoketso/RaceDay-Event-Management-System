
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Auth/login",
                new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            HttpContext.Session.SetString("JWTToken", result.GetProperty("token").GetString()!);
            HttpContext.Session.SetString("UserRole", result.GetProperty("role").GetString()!);
            HttpContext.Session.SetString("UserEmail", result.GetProperty("email").GetString()!);
            HttpContext.Session.SetString("UserID", result.GetProperty("userID").ToString());

            if (result.GetProperty("role").GetString() == "Organiser")
            {
                return RedirectToAction("Index", "Organiser");
            }

            return RedirectToAction("Index", "Participant");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string firstName,
            string lastName,
            string email,
            string password,
            string? phone)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Auth/register",
                new
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    Phone = phone
                });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration failed. Please check your details or use another email.";
                return View();
            }

            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
