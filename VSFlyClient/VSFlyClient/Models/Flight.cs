using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VSFlyClient.Models
{
    public class Flight
    {
        public int FlightID { get; set; }

        public string Departure { get; set; }

        public string Destination { get; set; }

        public DateTime Date { get; set; }

        public float BasePrice { get; set; }

        public bool Full { get; set; }
        public int TicketCount { get; set; }
        public short Seats { get; set; }
    }
}
