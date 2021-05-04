using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
using InventoryPOS.DataStore.Models.Interfaces;

namespace InventoryPOSApp.Core.Services
{
    public interface IInventoryService
    {
        bool AddColour(Colour colour);

        bool AddItemCategory(ItemCategory category);

        bool AddSize(Size size);

        bool AddBrand(Brand brand);

        long GenerateBarcode();

        bool AddProduct(Product product);
        List<IEnumerable<ProductAttribute>> GetProductAttributes();

        List<Product> GetAllProducts();
    }
}
