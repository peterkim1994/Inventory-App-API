using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class Payment
    {

        public int SaleInvoiceId { get; set; }
        public SaleInvoice SaleInvoice { get; set; }

        public int PaymentMethodId{ get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
