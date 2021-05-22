using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Dtos
{
    public class PaymentDto
    {
        public int SaleInvoiceId { get; set; }   
        [Required]
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
