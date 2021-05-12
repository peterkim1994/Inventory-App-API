using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore.Models
{

   // [Index(nameof(Type), IsUnique = true)]
    public class PromotionType
    {
        public int Id { get; set; }
      
        public string Type { get; set; }

    //    public virtual IList<Promotion> Promotions {get; set;}
    }
}
