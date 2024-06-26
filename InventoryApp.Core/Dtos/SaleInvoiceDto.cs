﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOSApp.Core.Dtos
{
    public class SaleInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string DateTime { get; set; }
        public IList<ProductSaleDto> Products { get; set; }
        public ICollection<PaymentDto> Payments { get; set; }
        public double Total { get; set; }
        public bool Finalised { get; set; }
        public double TotalCashRecieved { get; set; }
        public double ChangeGiven { get; set; }
        public IList<RefundDto> Refunds { get; set; }
    }
}
