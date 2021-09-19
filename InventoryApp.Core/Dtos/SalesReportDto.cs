using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOSApp.Core.Dtos
{
    public class SalesReportDto
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal EftposAmount { get; set; }

        public decimal CashAmount { get; set; }

        public decimal AfterPayAmount { get; set; }

        public decimal StoreCreditAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalRefunds { get; set; }

        public decimal NetTotal { get; set; }

    }
}
