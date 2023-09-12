using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class ProdactViewModel
    {
        public int productId { get; set; }

        public int brandId { get; set; }

        public string prductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal unitPrice { get; set; }

        public int supplierId { get; set; }

    }
}