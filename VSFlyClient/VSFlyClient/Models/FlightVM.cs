using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyClient.Models
{
    public class FlightVM
    {
        public int FlightID { get; set; }

        public string Departure { get; set; }

        public string Destination { get; set; }

        public DateTime Date { get; set; }

        public float PurchasePrice { get; set; }

    }
}
