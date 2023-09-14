//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventoryManagmentSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentMethod { get; set; }
        public int OrderCreateBy { get; set; }
    
        public virtual CustomerDetail CustomerDetail { get; set; }
        public virtual UserRegister UserRegister { get; set; }
        public virtual OrderStatu OrderStatu { get; set; }
        public virtual PayMentMethod PayMentMethod1 { get; set; }
    }
}
