using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class InvoiceViewModel
    {
        public CustomerDetailsModel CustomerDetails { get; set; }
        public List<InvoiceItemModel> InvoiceItems { get; set; } 
    }

    public class InvoiceItemModel
    {
        public int InvoiceId { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}