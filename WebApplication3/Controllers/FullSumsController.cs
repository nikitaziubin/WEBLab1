using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FullSumsController : ControllerBase
    {
        private readonly HotelContext _context;

        public FullSumsController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/FullSums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FullSum>>> GetFullSum()
        {
            if (_context.FullSum == null)
            {
                return NotFound();
            }
            return await _context.FullSum.ToListAsync();
        }

        // GET: api/FullSums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FullSum>> GetFullSum(int id)
        {
            if (_context.FullSum == null)
            {
                return NotFound();
            }
            var fullSum = await _context.FullSum.FindAsync(id);

            if (fullSum == null)
            {
                return NotFound();
            }

            return fullSum;
        }

        // PUT: api/FullSums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFullSum(int id, FullSum fullSum)
        {
            if (id != fullSum.Id)
            {
                return BadRequest();
            }

            _context.Entry(fullSum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FullSumExists(id))
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

        // POST: api/FullSums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<double>> PostFullSum(FullSum fullSum)
        {
            DateTime firstDate = (DateTime)fullSum.departureDate;
            double departureDate = firstDate.DayOfYear;
            DateTime secondDate = (DateTime)fullSum.arrivalDate;

			double arrivalDate = secondDate.DayOfYear;
            double difference = departureDate - arrivalDate;
            double FullPrice = difference * fullSum.oneNightPrice;
            _context.FullSum.Add(fullSum);
            return FullPrice;
        }

        // DELETE: api/FullSums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFullSum(int id)
        {
            if (_context.FullSum == null)
            {
                return NotFound();
            }
            var fullSum = await _context.FullSum.FindAsync(id);
            if (fullSum == null)
            {
                return NotFound();
            }

            _context.FullSum.Remove(fullSum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FullSumExists(int id)
        {
            return (_context.FullSum?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
