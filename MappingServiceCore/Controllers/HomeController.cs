using MappingServiceCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MappingServiceCore.Controllers
{
    public class HomeController : Controller
    {
        #region Ctor

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion

        //[Route("home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("mapping")]
        public IActionResult Mapping()
        {
            if (TempData["ErrorMessage"] != null)
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];

            return View();
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}