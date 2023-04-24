namespace WebApplication3.Models
{
    public class RoomType
    {
        RoomType()
        {
            Rooms = new List<Room>();
        }
        public int id { get; set; }
        public string? roomType { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}