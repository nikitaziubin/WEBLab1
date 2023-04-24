using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Client
    {
        Client()
        {
            Bookings = new List<Booking>();
        }
        public int id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Ім'я")]
        public string? name { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Номер телефону")]
        public string? phoneNumber { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}