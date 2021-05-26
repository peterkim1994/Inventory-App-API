using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public int GstNum { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

    }
}
