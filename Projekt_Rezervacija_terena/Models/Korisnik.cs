using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // DODANO: Potrebno za [NotMapped]

namespace Projekt_Rezervacija_terena.Models
{
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }


        // --- OVDJE JE DODANO NOVO POLJE ZA PONOVLJENU LOZINKU ---
        [NotMapped] // Govori bazi da ne stvara ovaj stupac u SQL-u
        [Required(ErrorMessage = "Molimo ponovite lozinku.")]
        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju.")]
        public string PonovljenaLozinka { get; set; }
    }
}