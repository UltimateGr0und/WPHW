using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WPHW1.Models;

namespace WPHW1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()//этот метод связан с index.cshtml
        {
            return View();//здесь мы возвращаем view, то есть саму cshtml разметку после всех действий.
        }

        public IActionResult Privacy()//этот метод связан с privacy.cshtml
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}