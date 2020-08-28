using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSFlyClient.Models;

namespace VSFlyClient.Factory
{
    public partial class ApiClient
    {
        public async Task<List<Flight>> GetAllFlights()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights"));
            return await GetAsync<List<Flight>>(requestUrl);
        }
        public async Task<List<Flight>> GetFlights()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/Available"));
            return await GetAsync<List<Flight>>(requestUrl);
        }
        public async Task<Flight> GetFlight(int id)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/" + id));
            return await GetAsync<Flight>(requestUrl);
        }
        
        public async Task<float> CalculatePrice(int id)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/CalculatePrice/" + id));
            return await GetAsync<float>(requestUrl);
        }
        public async Task<Message<Flight>> SaveFlight(Flight model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/"));
            return await PostAsync<Flight>(requestUrl, model);
        }

        public async Task<Message<Customer>> CreateCustomerAndBook(Customer model, int flightID, int purchasePrice)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Customers/CreateCustomerAndBook/" + flightID + "/" + purchasePrice));
            return await PostAsync<Customer>(requestUrl, model);
        }
        public async Task<float> TotalSalesForFlight(int flightId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/TotalSalesForFlight/" + flightId));
            return await GetAsync<float>(requestUrl);
        }
        public async Task<int> CountForFlight(int flightId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/CountForFlight/" + flightId));
            return await GetAsync<int>(requestUrl);
        }

        public async Task<float> AvgSaleForDestination(string destination)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Flights/AvgSaleForDestination/" + destination));
            return await GetAsync<float>(requestUrl);
        }
        
        public async Task<List<Booking>> DestinationBookings(string destination)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Bookings/TicketsForDestination/" + destination));
            return await GetAsync<List<Booking>>(requestUrl);
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "Customers/" + id));
            return await GetAsync<Customer>(requestUrl);
        }
    }
}
