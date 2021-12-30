using System.ComponentModel.DataAnnotations;

namespace InventoryPOS.DataStore.Daos
{
    public class ProductSale
    {    
        [Key]
        public int Id { get; set; }
        public int SaleInvoiceId { get; set; }
        public SaleInvoice SaleInvoice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool PromotionApplied{get;set;}
        public int ? PromotionId { get; set; }

        [MaxLength(50)]
        public string PromotionName { get; set; }
        public Promotion Promotion { get; set; }
        [Required]
        public int PriceSold { get; set; }
        public bool Canceled { get; set; }
        public bool Restocked { get; set; }
    }
}
