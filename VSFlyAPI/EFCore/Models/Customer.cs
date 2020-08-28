using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCore.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        public string Surname { get; set; }

        public string GivenName { get; set; }

        public virtual ICollection<Booking> BookingSet { get; set; }
    }
}
