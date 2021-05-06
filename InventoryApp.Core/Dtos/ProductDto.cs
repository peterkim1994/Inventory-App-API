using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Dtos;

namespace InventoryPOS.Core.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public long Barcode { get; set; }
        public string ManufactureCode { get; set; }       
        public string Description { get; set; }
     //   public int BrandId { get; set; }
        public string BrandValue { get; set; }
        public int ColourId { get; set; }
        public string ColourValue { get; set; }
        public string ItemCategoryValue { get; set; }
        public int SizeId { get; set; }
        public string SizeValue { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
    }
}
