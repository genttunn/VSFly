using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyClient.Models
{
    public class DestinationBookingsVM
    {
        public string Surname { get; set; }

        public string GivenName { get; set; }

        public int FlightID { get; set; }

        public float PurchasePrice { get; set; }

    }
}
