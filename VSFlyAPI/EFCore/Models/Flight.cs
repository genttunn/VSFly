using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCore.Models
{
    public class Flight
    {
        [Key]
        public int FlightID { get; set; }

        [StringLength(50), MinLength(3)]
        public string Departure { get; set; }

        [StringLength(50), MinLength(3)]
        public string Destination { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public float BasePrice { get; set; }

        [Required]
        public short? Seats { get; set; }

        public virtual ICollection<Booking> BookingSet { get; set; }

        [DefaultValue(false)]
        public bool Full { get; set; }
    }
}
