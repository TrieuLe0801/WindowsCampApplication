using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCampApplication.Model
{
    public class OrderInfo
    {
        // Properties.
        public string OrderLink { get; set; }
        public DateTime Time { get; set; }
        public string Size { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Card { get; set; }
        public string ExDate { get; set; }
        public string Security { get; set; }
    }
}
