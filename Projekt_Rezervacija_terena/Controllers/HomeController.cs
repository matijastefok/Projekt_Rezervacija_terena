using Microsoft.AspNetCore.Mvc;
using Projekt_Rezervacija_terena.Models;
using System.Diagnostics;

namespace Projekt_Rezervacija_terena.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

      
        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Prijava()
        {
            return View();
        }


        public IActionResult Registracija()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Rezerviraj(Rezervacija model)
        {
            
            if (ModelState.IsValid)
            {
                TempData["Poruka"] = "Rezervacija je uspješno poslana (simulacija)!";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}