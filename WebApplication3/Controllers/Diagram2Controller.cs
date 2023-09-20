using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers
{
	//[Authorize(Roles = "admin, user")]
	[Route("api/[controller]")]
	[ApiController]
	public class Diagram2Controller : ControllerBase
	{
		private readonly HotelContext _context;
		public Diagram2Controller(HotelContext context)
		{
			_context = context;
		}

		[HttpGet("JsonData")]
		public JsonResult JsonData()
		{
			//var categories = _context.Matches.Include(c => c.Goals).ToList();
			var hotelContext = _context.Bookings.Include(p => p.roomNavigation).ToList();
			var rooms = _context.Bookings.ToList();
			List<object> catBook = new List<object>();
			catBook.Add(new[] { "Room number", "Departure date" });

			foreach (var c in rooms)
			{
				if (c.roomNavigation.state == true)
				{
					catBook.Add(new object[] { c.departureDate, c.roomNavigation.roomNumber });
				}
			}

			return new JsonResult(catBook);
		}
	}
}