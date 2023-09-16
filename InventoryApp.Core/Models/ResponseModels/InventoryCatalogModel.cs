using InventoryPOS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPOSApp.Core.Models.ResponseModels
{
    public class InventoryCatalogModel
    {
        public int StoreId { get; set; }

        public IEnumerable<ProductDto> IncludedProducts { get; set; }

        public int AvailableNumberOfPages { get; set; }
    }
}