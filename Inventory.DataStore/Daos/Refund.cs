using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class Refund
    {
        [Key]
        public int Id { get; set; }
        public DateTime RefundDate {get;set;}
        [MaxLength(100)]
        public string Reason { get; set; }
        public int SaleInvoiceId { get; set; }
        public virtual SaleInvoice SaleInvoice {get;set;}
        public int ProductId { get; set; }
        public int Amount { get; set; }

    }
}
