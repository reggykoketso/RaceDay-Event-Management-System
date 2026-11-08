
using Microsoft.AspNetCore.Mvc;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserID");
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId) || role != "Participant")
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }
    }
}

