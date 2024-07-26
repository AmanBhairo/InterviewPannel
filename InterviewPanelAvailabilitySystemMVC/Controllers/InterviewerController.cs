using InterviewPanelAvailabilitySystemMVC.Infrastructure;
using InterviewPanelAvailabilitySystemMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPanelAvailabilitySystemMVC.Controllers
{
    public class InterviewerController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Index","Home");
            }
        }


    }
}
