using Microsoft.AspNetCore.Mvc;

namespace LeaveTrackerMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
