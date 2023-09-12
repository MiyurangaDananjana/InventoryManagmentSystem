﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class ProdactViewModel
    {
        public int ProductId { get; set; }

        public int BrandId { get; set; }

        public string ProductName    { get; set; }

        public string ProductDescription { get; set; }

        public decimal UnitPrice { get; set; }

        public int SupplierId { get; set; }

    }
}