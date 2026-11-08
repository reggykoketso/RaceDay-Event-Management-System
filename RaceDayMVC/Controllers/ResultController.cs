
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class ResultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ResultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

        private async Task<List<EnrolmentOptionViewModel>> GetEnrolmentOptions(
            HttpClient client)
        {
            var enrolments =
                await client.GetFromJsonAsync<List<EnrolmentViewModel>>(
                    "api/Enrolment") ?? new List<EnrolmentViewModel>();

            var events =
                await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Event") ?? new List<EventViewModel>();

            var categories =
                await client.GetFromJsonAsync<List<CategoryViewModel>>(
                    "api/Category") ?? new List<CategoryViewModel>();

            return enrolments.Select(e => new EnrolmentOptionViewModel
            {
                EnrolmentID = e.EnrolmentID,
                ParticipantID = e.ParticipantID,
                EventID = e.EventID,
                CategoryID = e.CategoryID,
                EventName = events.FirstOrDefault(x => x.EventID == e.EventID)
                    ?.Name ?? "Event unavailable",
                CategoryName = categories.FirstOrDefault(x => x.CategoryID == e.CategoryID)
                    ?.CategoryName ?? "Category unavailable"
            }).ToList();
        }

        // Participants see their own results.
        // Organisers see all results.
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var results =
                await client.GetFromJsonAsync<List<ResultViewModel>>(
                    "api/Result") ?? new List<ResultViewModel>();

            var enrolments = await GetEnrolmentOptions(client);

            if (!IsOrganiser())
            {
                var myEnrolmentIds = enrolments
                    .Select(e => e.EnrolmentID)
                    .ToHashSet();

                results = results
                    .Where(r => myEnrolmentIds.Contains(r.EnrolmentID))
                    .ToList();
            }

            foreach (var result in results)
            {
                var enrolment = enrolments.FirstOrDefault(
                    e => e.EnrolmentID == result.EnrolmentID);

                if (enrolment != null)
                {
                    result.ParticipantID = enrolment.ParticipantID;
                    result.EventName = enrolment.EventName;
                    result.CategoryName = enrolment.CategoryName;
                }
            }

            ViewBag.IsOrganiser = IsOrganiser();

            return View(results);
        }

        // Organiser: display the create form.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn() || !IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var model = new ResultCreateViewModel
            {
                Enrolments = await GetEnrolmentOptions(client)
            };

            return View(model);
        }

        // Organiser: save a new result.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultCreateViewModel model)
        {
            if (!IsLoggedIn() || !IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            model.Enrolments = await GetEnrolmentOptions(client);

            if (!TimeSpan.TryParse(model.FinishTime, out var finishTime)
                || finishTime < TimeSpan.Zero)
            {
                ModelState.AddModelError(
                    nameof(model.FinishTime),
                    "Enter a valid finishing time.");

                return View(model);
            }

            if (!model.Enrolments.Any(e => e.EnrolmentID == model.EnrolmentID))
            {
                ModelState.AddModelError(
                    nameof(model.EnrolmentID),
                    "Select a valid participant enrolment.");

                return View(model);
            }

            var newResult = new
            {
                model.EnrolmentID,
                FinishTime = finishTime,
                model.FinishPosition
            };

            var response = await client.PostAsJsonAsync(
                "api/Result", newResult);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Race result added successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                "", "The result could not be saved. Please try again.");

            return View(model);
        }

        // Organiser: display the edit form.
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn() || !IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var results =
                await client.GetFromJsonAsync<List<ResultViewModel>>(
                    "api/Result") ?? new List<ResultViewModel>();

            var result = results.FirstOrDefault(r => r.ResultID == id);

            if (result == null)
                return NotFound();

            var model = new ResultCreateViewModel
            {
                EnrolmentID = result.EnrolmentID,
                FinishTime = result.FinishTime,
                FinishPosition = result.FinishPosition,
                Enrolments = await GetEnrolmentOptions(client)
            };

            ViewBag.ResultID = id;

            return View(model);
        }

        // Organiser: update an existing result.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResultCreateViewModel model)
        {
            if (!IsLoggedIn() || !IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            model.Enrolments = await GetEnrolmentOptions(client);

            if (!TimeSpan.TryParse(model.FinishTime, out var finishTime)
                || finishTime < TimeSpan.Zero)
            {
                ModelState.AddModelError(
                    nameof(model.FinishTime),
                    "Enter a valid finishing time.");

                ViewBag.ResultID = id;
                return View(model);
            }

            if (!model.Enrolments.Any(e => e.EnrolmentID == model.EnrolmentID))
            {
                ModelState.AddModelError(
                    nameof(model.EnrolmentID),
                    "Select a valid participant enrolment.");

                ViewBag.ResultID = id;
                return View(model);
            }

            var updatedResult = new
            {
                ResultID = id,
                model.EnrolmentID,
                FinishTime = finishTime,
                model.FinishPosition
            };

            var response = await client.PutAsJsonAsync(
                $"api/Result/{id}", updatedResult);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Race result updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                "", "The result could not be updated.");

            ViewBag.ResultID = id;
            return View(model);
        }

        // Organiser: delete a result.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn() || !IsOrganiser())
                return RedirectToAction("Login", "Auth");

            var client = GetApiClient();

            var response = await client.DeleteAsync($"api/Result/{id}");

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode
                    ? "Race result deleted successfully."
                    : "The result could not be deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
