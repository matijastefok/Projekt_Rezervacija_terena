using System.ComponentModel.DataAnnotations;

namespace Projekt_Rezervacija_terena.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Odabir sporta je obavezan")]
        public string Sport { get; set; }

        [Required(ErrorMessage = "Molimo odaberite datum")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Molimo odaberite vrijeme")]
        [DataType(DataType.Time)]
        public TimeSpan Vrijeme { get; set; }
    }
}