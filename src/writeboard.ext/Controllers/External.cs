using Microsoft.AspNetCore.Mvc;

namespace writeboard.ext.Controllers
{
    public class External : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
