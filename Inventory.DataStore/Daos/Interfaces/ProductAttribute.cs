using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOS.DataStore.Daos.Interfaces
{
    public abstract class ProductAttribute
    {
        public abstract int Id { get; set; }
        public abstract string Value { get; set; }
     //   public abstract string GetValue();
    }
}
