using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Room
    {
        Room()
        {
            Bookings = new List<Booking>();
        }
        [Key]
        public int id { get; set; }
        public int roomNumber { get; set; }
        public int roomTypeId { get; set; }
        [ForeignKey("roomTypeId")]
        public RoomType? roomTypeNavigation { get; set; }
        public double oneNightPrice { get; set; }
        public bool state { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}