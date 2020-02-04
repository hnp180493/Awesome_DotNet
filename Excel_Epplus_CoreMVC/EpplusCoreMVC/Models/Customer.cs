using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EpplusCoreMVC.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [DisplayName("Khách hàng")]
        public string CustomerName { get; set; }
        [DisplayName("Thư điện tử")]
        public string CustomerEmail { get; set; }
        public string CustomerCountry { get; set; }
    }
}
