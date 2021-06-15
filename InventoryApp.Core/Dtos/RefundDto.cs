using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOSApp.Core.Dtos
{
    public class RefundDto
    {
        public int Id { get; set; }
        public DateTime RefundDate { get; set; }
        [MaxLength(100)]
        public string Reason { get; set; }
        public int SaleInvoiceId { get; set; }
        public double Amount { get; set; }
    }
}
