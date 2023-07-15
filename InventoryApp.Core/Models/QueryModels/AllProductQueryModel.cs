using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPOSApp.Core.Models.QueryModels
{
    public class AllProductQueryModel
    {
        public int PageNum { get; set; }

        public int StoreId { get; set; }

        public int NumItemsPerPage { get; set; }
    }
}