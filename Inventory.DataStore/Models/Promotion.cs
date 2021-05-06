using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore.Models
{
    
    public class Promotion
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [Required]  
        //QTY required to qualify for promotion, eg, a 2 for 1 promotion will have a Quantity of 2
        public int Quantity { get; set; }
        [Required]
        public int PromotionPrice { get; set; }

        public IList<ProductPromotion> ProductPromotions { get; set; }
    }
}
