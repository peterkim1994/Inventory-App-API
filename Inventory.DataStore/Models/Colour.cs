using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Models
{
    public class Colour
    {
        [Key]
        public int Id { get; set; }
        public string ColourName { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
