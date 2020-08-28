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
    public class CustomersController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public CustomersController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerSet()
        {
            return await _context.CustomerSet.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.CustomerSet.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
        // Create new customer and make new booking
        [HttpPost("CreateCustomerAndBook/{flightId}/{purchasePrice}")]
        public async Task<ActionResult<Customer>> CreateCustomerAndBook(Customer customer, int flightId, int purchasePrice)
        {
            _context.CustomerSet.Add(customer);
            await _context.SaveChangesAsync();

            Booking booking = new Booking();
            booking.CustomerID = customer.CustomerID;
            booking.FlightID = flightId;
            booking.PurchasePrice = purchasePrice;
            _context.BookingSet.Add(booking);
            await _context.SaveChangesAsync();

            Flight flight = await _context.FlightSet.FindAsync(flightId);
            var count = flight.BookingSet.Count;
            if (count >= booking.Flight.Seats)
            {
                flight.Full = true;
                _context.Entry(flight).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerID }, customer);
        }
    }
}
