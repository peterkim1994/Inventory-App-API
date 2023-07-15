using InventoryPOS.DataStore.Daos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPOSApp.Core.Models.QueryModels
{
    public class ProductSearchByAttributes
    {
        public int ColourId { get; set; }

        public int ProductAttribute { get; set; }
    }
}