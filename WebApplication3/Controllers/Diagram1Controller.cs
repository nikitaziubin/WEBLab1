using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authorization;

namespace WEBLab1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Diagram1Controller : ControllerBase
	{
		private readonly HotelContext _context;
		public Diagram1Controller(HotelContext context)
		{
			_context = context;
		}

		[HttpGet("JsonData")]
		public JsonResult JsonData()
		{
			//var categories = _context.Matches.Include(c => c.Goals).ToList();
			var hotelContext = _context.Bookings.Include(p => p.roomNavigation).ToList();
			var rooms = _context.Rooms.ToList();
			List<object> catBook = new List<object>();
			catBook.Add(new[] { "Room number", "price" });

			foreach (var c in rooms)
			{
				if (c.state == false)
				{
					catBook.Add(new object[] { c.roomNumber.ToString(), c.oneNightPrice});
				}
			}

			return new JsonResult(catBook);
		}
	}
}