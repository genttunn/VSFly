using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCore;
using EFCore.Models;

namespace VSFlyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public BookingsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingSet()
        {
            return await _context.BookingSet.ToListAsync();
        }

        // GET: api/Bookings/Calculate
        //Takes a destination, search for all flights with that destination,return all flights respective bookings as list
        [HttpGet("TicketsForDestination/{destination}")]
        public async Task<ActionResult<List<Booking>>> TicketsForDestination(string destination)
        {
            List<Flight> flights = await _context.FlightSet.Where(f => f.Destination == destination).ToListAsync();
            List<Booking> bookings = new List<Booking>();
            foreach (Flight f in flights)
            {
                foreach (Booking b in f.BookingSet)
                {
                    bookings.Add(b);
                }
            }
            return bookings;
        }


        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.BookingSet.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }
    }
}
