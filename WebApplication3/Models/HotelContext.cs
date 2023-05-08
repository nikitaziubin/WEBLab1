using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication3.Models;

namespace WebApplication3.Models
{
    public class HotelContext : DbContext
    {
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Client> Clients { get; set; }

        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WebApplication3.Models.FullSum> FullSum { get; set; } = default!;
    }
}
