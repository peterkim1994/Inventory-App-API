using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOS.DataStore.Models
{
    public class SaleInvoice
    {
        [Key]        
        public int Id { get; set; }       
        public int StoreId { get; set; }
        public Store store { get; set; }
        public DateTime InvoiceDate { get; set; }
        public virtual IList<ProductSale> ProductSales { get; set; }
        public virtual IList<Payment> Payments { get; set; }
        public int Total { get; set; }
        public bool Finalised { get; set; }

    }
}
