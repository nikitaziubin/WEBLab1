
namespace WebApplication3.Models
{
    public class PaymentType
    {
        public PaymentType()
        {
            Payments = new List<Payment>();
        }
        public int id { get; set; }
        public string? paymentType { get; set; }
        public virtual ICollection<Payment> Payments { get; }
    }
}
