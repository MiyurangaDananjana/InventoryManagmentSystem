using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class SupplierDetailsModel
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

    }
}