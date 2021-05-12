using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class SaleInvoice
    {
        [Key]
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }        
        public List<ProductSale> ProductSales { get; set; }
        public List<Payment> Payments { get; set; }
        public bool Finalised { get; set; }

    }
}
