using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class CustomerView
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}