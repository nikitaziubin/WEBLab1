using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication3.Models
{
    public class Payment
    {
        public int id { get; set; }
        public int bookingIid { get; set; }
        public virtual Booking? BookingNavigation { get; set; }
        public int typeId { get; set; }
        public virtual PaymentType PaymentType { get; set; } = null!;

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Дата оплати")]
        public DateTime paymentDate { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути пустим")]
        [Display(Name = "Сумма")]
        public double amount { get; set; }

    }
}