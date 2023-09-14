using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalProductVariants { get; set; }
        public int TotalProducts { get; set; }
        public int TotalSystemUsers { get; set; }

    }
}