using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string SizeName { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
