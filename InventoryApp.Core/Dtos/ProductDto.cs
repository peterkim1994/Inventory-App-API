using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;

namespace InventoryPOS.Core.Dtos
{
    public class ProductDto
    {
        [Required]
        public int Id { get; set; }
        public long Barcode { get; set; }
        public string ManufactureCode { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public string BrandValue { get; set; }
        public int ColourId { get; set; }
        public string ColourValue { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategoryValue { get; set; }
        public int SizeId { get; set; }
        public string SizeValue { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }

        [Required]
        public int StoreId { get; set; }
        public bool Active { get; set; }
    }
}
