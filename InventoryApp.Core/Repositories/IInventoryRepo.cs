using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
using InventoryPOS.DataStore.Models.Interfaces;

namespace InventoryPOSApp.Core.Repositories
{
    public interface IInventoryRepo
    {
        void AddColour(Colour colour);

        void SaveChanges();

        void AddItemCategory(ItemCategory category);

        void AddNewProduct(Product product);

        void AddSize(Size size);

        bool ContainsAtt<T>(T newAtt) where T : ProductAttribute;



    }
}
