using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore.Models
{
    [Index(nameof(PromotionName), IsUnique = true)]
    public class Promotion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string PromotionName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]  
        //QTY required to qualify for promotion, eg, a 2 for 1 promotion will have a Quantity of 2
        public int Quantity { get; set; }
        [Required]
        public int PromotionPrice { get; set; }
        public virtual List<ProductPromotion> ProductPromotions { get; set; }
        
        public bool Active { get; set; }
    }
}
