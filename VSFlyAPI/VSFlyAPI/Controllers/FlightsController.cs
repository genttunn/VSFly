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
    public class FlightsController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public FlightsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightSet()
        {
            return await _context.FlightSet.ToListAsync();
        }
        // GET: api/Flights/Available
        // Retrieve only flights that are not full yet, and flight date is in the future
        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAvailableFlightSet()
        {
            DateTime presentTime = DateTime.Now;
            return await _context.FlightSet.Where(f => f.Full == false && f.Date > presentTime ).ToListAsync();
        }

        // GET: api/Flights/CalculatePrice/{id}
        // occupancyRate > 80% increase price 150%
        // Flight date less than 1 month (30 days) away and occupancyRate < 50% discount 30%
        // Flight date less than 2 months away and occupancyRate < 20% discount 20%
        // Else normal price (e.g : date < 1 month but > 50 occupancy is still full price )
        [HttpGet("CalculatePrice/{id}")]
        public async Task<ActionResult<double>> CalculatePurchasePriceForFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id);
            double count = (double)flight.BookingSet.Count;
            double seats = (double)flight.Seats;
            double occupancyRate = (count / seats) * 100;
            DateTime presentTime = DateTime.Now;
            long positiveTimeDifference = (flight.Date.Ticks - presentTime.Ticks);
            if (positiveTimeDifference < 0)
            {
                positiveTimeDifference = positiveTimeDifference * (-1);
            }
            TimeSpan elapsedSpan = new TimeSpan(positiveTimeDifference);
            double finalPrice = 0;

            if (occupancyRate >= 80)
            {
                finalPrice = flight.BasePrice * 1.5;
            }
            else if (elapsedSpan.TotalDays < 30 && occupancyRate < 50)
            {
                finalPrice = flight.BasePrice * 0.7;
            }
            else if (elapsedSpan.TotalDays < 60 && occupancyRate < 20)
            {
                finalPrice = flight.BasePrice * 0.8;
            }
            else {
                finalPrice = flight.BasePrice;
            }
            return Math.Round(finalPrice,0);
        }

        // GET: api/Flights/TotalSalesForFlight/{id} return total purchase price of flight' bookings
        [HttpGet("TotalSalesForFlight/{id}")]
        public async Task<ActionResult<double>> TotalSalesForFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id);
            double finalSale = 0.0;
            foreach (Booking b in flight.BookingSet) {
                finalSale += b.PurchasePrice;
            }
            return finalSale;
        }

        // GET: api/Bookings/CountForFlight/{id}
        //Takes a destination, search for all flights with that destination,return all flights respective bookings as list
        [HttpGet("CountForFlight/{id}")]
        public async Task<ActionResult<int>> CountForFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id);
            return flight.BookingSet.Count;
        }

        // GET: api/Flights/AvgSaleForDestination/{destination}
        // return average purchase price of destination's bookings
        [HttpGet("AvgSaleForDestination/{destination}")]
        public async Task<ActionResult<double>> AvgSaleForDestination(string destination)
        {
            List<Flight> flights = await _context.FlightSet.Where(f => f.Destination == destination).ToListAsync();
            double finalSale = 0.0;
            double totalBookings = 0.0;
            foreach (Flight f in flights) {
                foreach (Booking b in f.BookingSet)
                {
                    finalSale += b.PurchasePrice;
                    totalBookings += 1;
                }
            }
            if (totalBookings == 0) { return 0; }
            double avgSale = finalSale / totalBookings;
            return Math.Round(avgSale, 2, MidpointRounding.ToEven);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return flight;
        }

        // POST: api/Flights
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.FlightSet.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FlightID }, flight);
        }
    }
}
