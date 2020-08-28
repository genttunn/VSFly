using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Models
{
    public class Booking
    {
        public int FlightID { get; set; }
        public int CustomerID { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Customer Customer { get; set; }

        public float PurchasePrice { get; set; }
    }
}
