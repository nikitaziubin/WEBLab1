using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class FullSum
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public string? departureDate { get; set; }
        [NotMapped]
        public string? arrivalDate { get; set; }
        [NotMapped]
        public double oneNightPrice { get; set; }
    }
}
