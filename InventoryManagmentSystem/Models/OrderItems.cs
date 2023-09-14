using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class OrderItems
    {

        public int BrandId { get; set; }
        
        public int ProductId { get; set; }
        
        public int ProductVariantId { get; set; }
        
        public decimal ItemPrice { get; set; }
        
        public int Quantity { get; set; }

        public int PaymentMethod { get; set; }

    }
}