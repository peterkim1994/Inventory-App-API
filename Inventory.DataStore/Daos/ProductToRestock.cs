using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryPOS.DataStore.Daos
{
    public class ProductToRestock 
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public DateTime? DateReStocked { get; set; }
    }
}
