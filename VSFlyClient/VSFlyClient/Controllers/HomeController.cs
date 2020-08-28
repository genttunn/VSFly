using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VSFlyClient.Factory;
using VSFlyClient.Models;
using VSFlyClient.Utility;

namespace VSFlyClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<MySettingsModel> appSettings;
        public HomeController(ILogger<HomeController> logger, IOptions<MySettingsModel> app)
        {
            appSettings = app;
            ApplicationSettings.WebApiUrl = appSettings.Value.WebApiBaseUrl;
            _logger = logger;
        }
        //retrieve all flights, append column Remaining Seats
        public async Task<IActionResult> Index()
        {
            var data = await ApiClientFactory.Instance.GetFlights();
            foreach (Flight f in data) { 
                int booked = await ApiClientFactory.Instance.CountForFlight(f.FlightID);
                f.TicketCount = f.Seats - booked;
            }
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Flight flight)
        {
            await ApiClientFactory.Instance.SaveFlight(flight);
            return RedirectToAction("Index", "Home");
        }
        //Return flight info and Purchase Price
        public async Task<IActionResult> Book(int flightId) {
            HttpContext.Session.SetInt32("CurrentFlight", flightId);
            Flight flight = await ApiClientFactory.Instance.GetFlight(flightId);
            FlightVM flightVM = new FlightVM();

            flightVM.FlightID = flight.FlightID;
            flightVM.Departure = flight.Departure;
            flightVM.Destination = flight.Destination;
            flightVM.Date = flight.Date;
            flightVM.PurchasePrice = await ApiClientFactory.Instance.CalculatePrice(flightId);
            HttpContext.Session.SetInt32("CurrentPurchasePrice", Convert.ToInt32(flightVM.PurchasePrice));
            return View(flightVM);
        }
        //Create new customer and book the flight with them
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(Customer customer)
        {
            var currentFlightID = (int) HttpContext.Session.GetInt32("CurrentFlight");
            var currentPurchasePrice  = (int)  HttpContext.Session.GetInt32("CurrentPurchasePrice");
            await ApiClientFactory.Instance.CreateCustomerAndBook(customer, currentFlightID, currentPurchasePrice);
            return RedirectToAction("Index", "Home");
        }
        //Menu for viewing 3 statistics
        public IActionResult Statistics()
        {
            return View();
        }
        //Total sales for ALL flights (available or not)
        public async Task<IActionResult> TicketSales()
        {
            List<Flight> flights = await ApiClientFactory.Instance.GetAllFlights();
            List<TicketSalesVM> ticketSalesVMList = new List<TicketSalesVM>();

            foreach (Flight f in flights) {
                TicketSalesVM ticketSalesVM = new TicketSalesVM() { FlightID = f.FlightID, Departure = f.Departure, Destination = f.Destination, BasePrice = f.BasePrice, Date = f.Date, Full = f.Full, Seats = f.Seats };
                ticketSalesVM.TotalSales = await ApiClientFactory.Instance.TotalSalesForFlight(f.FlightID);
                ticketSalesVM.TicketCount = await ApiClientFactory.Instance.CountForFlight(f.FlightID);
                ticketSalesVMList.Add(ticketSalesVM);
            }
            return View(ticketSalesVMList);
        }
        //Destionation average ticket purchase price
        public async Task<IActionResult> DestinationAvg()
        {
            List<Flight> flights = await ApiClientFactory.Instance.GetAllFlights();
            List<DestinationAverageVM> destinationAverageVMList = new List<DestinationAverageVM>();
            List<string> destinationStrings = new List<string>();
            foreach (Flight f in flights)
            {
                if (!destinationStrings.Contains(f.Destination)) {
                    destinationStrings.Add(f.Destination);
                    DestinationAverageVM destinationAverageVM = new DestinationAverageVM() {DestinationName = f.Destination};
                    destinationAverageVM.AverageSales = await ApiClientFactory.Instance.AvgSaleForDestination(f.Destination);
                    destinationAverageVMList.Add(destinationAverageVM);
                } 
            }
            return View(destinationAverageVMList);
        }
        //Return list of destinations
        public async Task<IActionResult> DestinationBookings() {
            List<Flight> flights = await ApiClientFactory.Instance.GetAllFlights();
            List<string> destinationStrings = new List<string>();
            List<Destination> destinationsList = new List<Destination>();
            foreach (Flight f in flights)
            {
                if (!destinationStrings.Contains(f.Destination))
                {
                    destinationStrings.Add(f.Destination);
                    Destination dest = new Destination() { DestinationName = f.Destination };
                    destinationsList.Add(dest);
                }
            }
            return View(destinationsList);
        }
        //Return bookings by destination
        public async Task<IActionResult> DestinationBookingsDetails(string destination) {
            List<Booking> bookings = await ApiClientFactory.Instance.DestinationBookings(destination);
            List<DestinationBookingsVM> destinationBookingsVMList = new List<DestinationBookingsVM>();
            foreach (Booking b in bookings) {
                Customer cust = await ApiClientFactory.Instance.GetCustomer(b.CustomerID);
                DestinationBookingsVM destBookVM = new DestinationBookingsVM() { FlightID = b.FlightID,Surname = cust.Surname,GivenName = cust.GivenName, PurchasePrice = b.PurchasePrice };
                destinationBookingsVMList.Add(destBookVM);
            }
            return View(destinationBookingsVMList);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
