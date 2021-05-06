using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class ProductPromotion
    {

        public  int PromotionId { get; set; }
        public Promotion Promotion { get; set; }  
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
