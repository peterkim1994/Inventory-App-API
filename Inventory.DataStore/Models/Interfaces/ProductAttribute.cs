using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryPOS.DataStore.Models.Interfaces
{
    public abstract class ProductAttribute
    {
        public abstract string Value { get; set; }
     //   public abstract string GetValue();
    }
}
