using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.Core.Dtos;

namespace InventoryPOSApp.Core.Dtos
{
    public class ProductSaleDto
    {
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public bool PromotionApplied { get; set; }
        public int PromotionId { get; set; }        
        public string PromotionName { get; set; }
        public double PromotionPrice{ get; set; }
        public int PromotionQuantity { get; set; }
        public double PriceSold { get; set; }
    }
}
