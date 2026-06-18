using System.ComponentModel.DataAnnotations;

namespace Projekt_Rezervacija_terena.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Molimo upišite ili odaberite sport")]
        public string Sport { get; set; }

        [Required(ErrorMessage = "Upišite točan datum termina")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Upišite točno vrijeme termina")]
        [DataType(DataType.Time)]
        public TimeSpan Vrijeme { get; set; }

        public string KorisnikIme { get; set; } = string.Empty;
    }
}