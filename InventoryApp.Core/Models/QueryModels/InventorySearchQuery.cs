using InventoryPOS.DataStore.Daos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPOSApp.Core.Models.QueryModels
{
    public class InventorySearchQuery
    {
        public int StoreId { get; set; }

        public int? Colour { get; set; }

        public int? Size { get; set; }

        public int? Category { get; set; }

        public int? Brand { get; set; }

        public string? SearchString { get; set; }
    }
}