using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyClient.Models
{
    public class Booking
    {
        public int FlightID { get; set; }
        public int CustomerID { get; set; }
        public float PurchasePrice { get; set; }
    }
}
