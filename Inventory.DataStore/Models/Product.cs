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
        [Required]
        [MinLength(10),MaxLength(100)]
        public string Description
        { 
            get { return Description; }
            set { Description = String.Format("{0}, {1}, {2}, {3}, {4}", value, Brand, ItemCategory, Colour, Size); }
        }
        [Required]
        [MinLength(3)]
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
