using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using WebApplication3.Models;


namespace WebApplication3.Controllers
{
	public class EmailRequest
	{
		public string To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}
	[Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly HotelContext _context;

		private const string TopicName = "Email";



		public BookingsController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
            return await _context.Bookings.Include(p=>p.clientNavigation).Include(p=>p.roomNavigation).ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
          if (_context.Bookings == null)
          {
              return NotFound();
          }
			var booking = await _context.Bookings.Include(b=>b.clientNavigation).FirstOrDefaultAsync(b => b.roomId == id);


			if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
          if (_context.Bookings == null)
          {
              return Problem("Entity set 'HotelContext.Bookings'  is null.");
          }
            var room = _context.Rooms.FirstOrDefault(r => r.id == booking.roomNavigation.id);
            room.state = true;
            booking.roomNavigation = null;
            _context.Bookings.Add(booking);
            ProduceMessage(room.roomNumber, booking.arrivalDate, booking.departureDate);
            ConsumeAndSendEmail();
            await _context.SaveChangesAsync();
			return CreatedAtAction("GetBooking", new { id = booking.id }, booking);
        }

		static void ProduceMessage(int roomNUM, DateTime arrivalDate, DateTime departureDate)
		{
			var config = new ProducerConfig
			{
				BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
				SecurityProtocol = SecurityProtocol.SaslSsl,
				SaslMechanism = SaslMechanism.Plain,
				SaslUsername = "**************",
				SaslPassword = "*********************************",
			};

			using var producer = new ProducerBuilder<Null, string>(config).Build();
            var emailMessage = JsonConvert.SerializeObject(new
            {
            	To = "recipient@example.com",
            	Subject = "Сonfirmation of successful booking",
            	Body = $"Hello Mr Nikita, we would like to inform you that you have successfully booked room {roomNUM} from {arrivalDate} to {departureDate}"
            });
            //string emailMessage = "test";

			producer.Produce(TopicName, new Message<Null, string> { Value = emailMessage });
			producer.Flush();
		}

		static void ConsumeAndSendEmail()
		{
			var consumerConfig = new ConsumerConfig
			{
				GroupId = "email-group",
				BootstrapServers = "pkc-56d1g.eastus.azure.confluent.cloud:9092",
				SecurityProtocol = SecurityProtocol.SaslSsl,
				SaslMechanism = SaslMechanism.Plain,
				SaslUsername = "**************",
				SaslPassword = "*********************************",
			};

			using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
			consumer.Subscribe(TopicName);

			var consumeResult = consumer.Consume();
			var emailRequest = JsonConvert.DeserializeObject<EmailRequest>(consumeResult.Value);
			SendEmail(emailRequest);
		}

		static void SendEmail(EmailRequest emailRequest)
		{
			MimeMessage message = new MimeMessage();
			message.From.Add(new MailboxAddress("Nikita", "test.sending@ukr.net"));
			message.To.Add(new MailboxAddress("Recipient Name", "test.sendeng@gmail.com"));
			message.Subject = emailRequest.Subject;
			message.Body = new TextPart("Plain") { Text = emailRequest.Body };


			using var client = new MailKit.Net.Smtp.SmtpClient();
			client.Connect("smtp.ukr.net", 465, true);
			client.Authenticate("test.sending@ukr.net", "xUj2RdtgRQwXTm3y");
			client.Send(message);
		}


		// DELETE: api/Bookings/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }   

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
