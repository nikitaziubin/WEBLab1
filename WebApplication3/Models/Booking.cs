using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Booking
    {
        public int id { get; set; }
        public int clientId { get; set; }
        public Client? clientNavigation { get; set; }
        public int roomId { get; set; }
        public Room? roomNavigation { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Дата заїзду")]
        public DateTime arrivalDate { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Дата виїзду")]
        public DateTime departureDate { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Сумма бронювання")]
        public DateTime totalPrice { get; set; }
        public virtual Payment? Payments { get; set; }
    }
}
