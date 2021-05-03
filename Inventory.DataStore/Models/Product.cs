using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore.Models
{
    [Index(nameof(Barcode), IsUnique = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public long Barcode { get; set; }
        public string ManufactureCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public Brand Brand{ get; set; }
        public Colour Colour { get; set; }
        [Required]
        public ItemCategory ItemCategory { get; set; }
        [Required]
        public Size Size { get; set; }
        public int Price { get; set; }        
        public int Qty { get; set; }

    }
}
