using System.ComponentModel.DataAnnotations;

namespace MVC_DB_Mit_Bild.Models
{
    public class Person
    {
        public int PID { get; set; }
        [Required]
        public string Vorname { get; set; }
        [Required]
        public string Nachname { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public int? groesse { get; set; }
        public string? Bild { get; set; }
        public double? Gewicht { get; set; }
        public IFormFile? Uploaddatei { get; set; }
    }
}
