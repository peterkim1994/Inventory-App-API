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
        [MaxLength(20)]
        public string GstNum { get; set; }
        [MaxLength(30)]
        public string StoreName { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Contact { get; set; }
    }
}
