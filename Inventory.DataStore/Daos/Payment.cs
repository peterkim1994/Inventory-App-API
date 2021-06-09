using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class Payment
    {
        public int SaleInvoiceId { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
        public int PaymentMethodId{ get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
