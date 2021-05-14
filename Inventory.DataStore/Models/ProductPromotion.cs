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
        public virtual Promotion Promotion { get; set; }  
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
