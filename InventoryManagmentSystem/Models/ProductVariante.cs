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
    
    public partial class ProductVariante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductVariante()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int ProductVariantId { get; set; }
        public int ProductsId { get; set; }
        public string VariantDescription { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public Nullable<decimal> PriceModifier { get; set; }
        public Nullable<int> StockQuantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Product Product { get; set; }
        public virtual UserRegister UserRegister { get; set; }
    }
}