using Microsoft.AspNetCore.Mvc;
using Projekt_Rezervacija_terena.Data; // Poveznica s bazom podataka
using Projekt_Rezervacija_terena.Models;
using System.Diagnostics;

namespace Projekt_Rezervacija_terena.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Varijabla za bazu podataka

        // Konstruktor kroz koji ubacujemo logger i našu bazu podataka
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // 1. POČETNA STRANICA
        public IActionResult Index()
        {
            return View();
        }

        // 2. PRIJAVA (Prikaz stranice)
        public IActionResult Prijava()
        {
            return View();
        }

        // 2. PRIJAVA (Akcija kada korisnik klikne gumb za prijavu)
        [HttpPost]
        public IActionResult Prijava(string email, string lozinka)
        {
            // 1. Prvo tražimo postoji li uopće korisnik s tim emailom
            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Email == email);

            if (korisnik == null)
            {
                // Ako email ne postoji u bazi, ispisujemo jasnu poruku
                ViewBag.Greska = "Korisnik s ovom email adresom nije registriran.";
                return View();
            }

            // 2. Ako korisnik postoji, provjeravamo poklapa li se lozinka
            if (korisnik.Lozinka != lozinka)
            {
                // Ako je lozinka kriva, javljamo točnu grešku
                ViewBag.Greska = "Kriva lozinka. Pokušajte ponovno.";
                return View();
            }

            // 3. Ako je sve točno, spremamo poruku i šaljemo korisnika na početnu stranicu (Index)
            HttpContext.Session.SetString("KorisnikIme", korisnik.Ime);
            TempData["Poruka"] = $"Uspješno ste se prijavili kao {korisnik.Ime}!";
            return RedirectToAction("Index");
        }

        // 3. REGISTRACIJA (Prikaz stranice)
        public IActionResult Registracija()
        {
            return View();
        }

        // 3. REGISTRACIJA (Akcija kada korisnik klikne gumb za registraciju)
        [HttpPost]
        public IActionResult Registracija(Korisnik noviKorisnik)
        {
            if (ModelState.IsValid)
            {
                // Provjera postoji li već netko s tim emailom u bazi
                var postoji = _context.Korisnici.Any(k => k.Email == noviKorisnik.Email);
                if (postoji)
                {
                    ViewBag.Greska = "Korisnik s ovim Emailom već postoji!";
                    return View(noviKorisnik);
                }

                // Spremanje novog korisnika u bazu podataka
                _context.Korisnici.Add(noviKorisnik);
                _context.SaveChanges();

                TempData["Poruka"] = "Registracija uspješna! Sada se možete prijaviti.";
                return RedirectToAction("Prijava");
            }

            // Ako validacija modela nije prošla (npr. lozinke se ne podudaraju), vraćamo formu s greškama
            return View(noviKorisnik);
        }

        // 4. O PROJEKTU (Privacy)
        public IActionResult Privacy()
        {
            return View();
        }

        // 5. REZERVACIJA TERENA (Akcija kada korisnik sprema rezervaciju)
        [HttpPost]
        public IActionResult Rezerviraj(Rezervacija model)
        {
            _context.Rezervacije.Add(model);
            _context.SaveChanges(); // Spremanje u bazu podataka

            TempData["Poruka"] = "Uspješno ste rezervirali termin!";
            return RedirectToAction("Index");
        }

        // 6. ODJAVA KORISNIKA (Brisanje sesije)
        public IActionResult Odjava()
        {
            HttpContext.Session.Clear(); // Briše sve podatke iz sesije (korisnik više nije prijavljen)
            TempData["Poruka"] = "Uspješno ste se odjavili.";
            return RedirectToAction("Index"); // Vraća korisnika na početnu stranicu
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}