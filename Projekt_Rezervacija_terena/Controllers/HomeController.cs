using Microsoft.AspNetCore.Mvc;
using Projekt_Rezervacija_terena.Data;
using Projekt_Rezervacija_terena.Models;
using System.Diagnostics;

namespace Projekt_Rezervacija_terena.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // 1. POČETNA STRANICA - Prikazuje rezervacije samo za prijavljenog korisnika
        public IActionResult Index()
        {
            var prijavljeniKorisnik = HttpContext.Session.GetString("KorisnikIme");

            if (string.IsNullOrEmpty(prijavljeniKorisnik))
            {
                return View(new List<Rezervacija>());
            }

            var korisnikoveRezervacije = _context.Rezervacije
                                                 .Where(r => r.KorisnikIme == prijavljeniKorisnik)
                                                 .ToList();

            return View(korisnikoveRezervacije);
        }

        // 2. ADMIN PANEL - Vidljiv samo ako je prijavljen admin
        public IActionResult Admin()
        {
            var prijavljeniEmail = HttpContext.Session.GetString("KorisnikEmail");

            // Ovdje definiramo tko ima pravo pristupa Admin Panelu
            if (string.IsNullOrEmpty(prijavljeniEmail) || prijavljeniEmail != "admin@primjer.com")
            {
                TempData["Greska"] = "Nemate ovlasti za pristup Admin Panelu!";
                return RedirectToAction("Index");
            }

            // Admin povlači APSOLUTNO SVE rezervacije iz baze podataka
            var sveRezervacije = _context.Rezervacije.ToList();
            return View(sveRezervacije);
        }

        public IActionResult Prijava()
        {
            return View();
        }

        // 3. PRIJAVA - Dodano spremanje Emaila u sesiju radi provjere admina
        [HttpPost]
        public IActionResult Prijava(string email, string lozinka)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lozinka))
            {
                ViewBag.Greska = "Molimo unesite email i lozinku!";
                return View();
            }

            if (!email.Contains("@"))
            {
                ViewBag.Greska = "Molimo unesite ispravnu email adresu (npr. ime@primjer.com).";
                return View();
            }

            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Email == email);

            if (korisnik == null)
            {
                ViewBag.Greska = "Korisnik s ovom email adresom nije registriran.";
                return View();
            }

            if (korisnik.Lozinka != lozinka)
            {
                ViewBag.Greska = "Kriva lozinka. Pokušajte ponovno.";
                return View();
            }

            // U sesiju spremamo i Ime i Email
            HttpContext.Session.SetString("KorisnikIme", korisnik.Ime);
            HttpContext.Session.SetString("KorisnikEmail", korisnik.Email);

            TempData["Poruka"] = $"Uspješno ste se prijavili kao {korisnik.Ime}!";
            return RedirectToAction("Index");
        }

        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registracija(Korisnik noviKorisnik)
        {
            if (ModelState.IsValid)
            {
                var postoji = _context.Korisnici.Any(k => k.Email == noviKorisnik.Email);
                if (postoji)
                {
                    ViewBag.Greska = "Korisnik s ovim Emailom već postoji!";
                    return View(noviKorisnik);
                }

                _context.Korisnici.Add(noviKorisnik);
                _context.SaveChanges();

                TempData["Poruka"] = "Registracija uspješna! Sada se možete prijaviti.";
                return RedirectToAction("Prijava");
            }

            return View(noviKorisnik);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // 5. REZERVACIJA TERENA
        [HttpPost]
        public IActionResult Rezerviraj(Rezervacija model)
        {
            var prijavljeniKorisnik = HttpContext.Session.GetString("KorisnikIme");

            if (string.IsNullOrEmpty(prijavljeniKorisnik))
            {
                TempData["Greska"] = "Morate se prijaviti da biste rezervirali teren!";
                return RedirectToAction("Prijava");
            }

            if (model.Datum == DateTime.MinValue || model.Vrijeme == TimeSpan.Zero || string.IsNullOrEmpty(model.Sport))
            {
                TempData["Greska"] = "Molimo odaberite točan datum i vrijeme termina!";
                return RedirectToAction("Index");
            }

            model.KorisnikIme = prijavljeniKorisnik;

            _context.Rezervacije.Add(model);
            _context.SaveChanges();

            TempData["Poruka"] = "Uspješno ste rezervirali termin!";
            return RedirectToAction("Index");
        }

        // 6. KORISNIČKO OTKAZIVANJE REZERVACIJE
        [HttpPost]
        public IActionResult OtvaziRezervaciju(int id)
        {
            var rezervacija = _context.Rezervacije.Find(id);

            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
                _context.SaveChanges();
                TempData["Poruka"] = "Uspješno ste otkazali termin!";
            }

            return RedirectToAction("Index");
        }

       
        [HttpPost]
        public IActionResult OtkaziBiloKojuRezervaciju(int id)
        {
            var rezervacija = _context.Rezervacije.Find(id);

            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
                _context.SaveChanges();
                TempData["Poruka"] = "Rezervacija uspješna uklonjena od strane administratora.";
            }

            return RedirectToAction("Admin");
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Clear();
            TempData["Poruka"] = "Uspješno ste se odjavili.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}