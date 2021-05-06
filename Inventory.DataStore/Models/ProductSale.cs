using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class ProductSale
    {

        public int SalesInvoiceId { get; set; }
        public SaleInvoice SaleInvoice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int PriceSold { get; set; }
    }
}
