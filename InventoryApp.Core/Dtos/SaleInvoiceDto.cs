using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOSApp.Core.Dtos
{
    public class SaleInvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public string DateTime { get; set; }
        public ICollection<ProductSaleDto> Products {get; set; }
        public ICollection<PaymentDto> Payments { get; set; }
        public double Total { get; set; }
    }
}
