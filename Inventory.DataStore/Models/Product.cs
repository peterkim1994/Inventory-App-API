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
        [MinLength(10),MaxLength(100)]
        public string Description { get; set; }

        public int BrandId { get; set; }
        public Brand Brand{ get; set; }
        public int ColourId { get; set; }
        public Colour Colour { get; set; }
        [Required]
        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }
        [Required]
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int Price { get; set; }        
        public int Qty { get; set; }
    }
}
