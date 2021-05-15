using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Dtos
{
    public class PromotionDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string PromotionName { get; set; }
        public int Quantity { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public double PromotionPrice { get; set; }
        public IList<int> ProductIds { get; set; }

    }
}
