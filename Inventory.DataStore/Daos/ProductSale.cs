using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class ProductSale
    {    
        [Key]
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public SaleInvoice SaleInvoice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool PromotionApplied{get;set;}
        public int ? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        [Required]
        public int PriceSold { get; set; }

    }
}
