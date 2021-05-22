using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class SaleInvoice
    {
        [Key]        
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public virtual IList<ProductSale> ProductSales { get; set; }
        public virtual IList<Payment> Payments { get; set; }
        public int Total { get; set; }
        public bool Finalised { get; set; }

    }
}
