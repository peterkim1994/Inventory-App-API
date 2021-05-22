using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;

namespace InventoryPOSApp.Core.Services
{
    public interface IInventoryService
    {
        bool AddColour(Colour colour);

        bool AddItemCategory(ItemCategory category);

        bool AddSize(Size size);

        bool AddBrand(Brand brand);

        bool EditBrand(Brand brand);

        bool EditCategory(ItemCategory category);

        bool EditSize(Size size);

        bool EditColour(Colour colour);

        long GenerateBarcode();

        bool AddProduct(Product product);
        List<IEnumerable<ProductAttribute>> GetProductAttributes();

        List<Product> GetAllProducts();

        List<Product> GetProducts(List<int> productIds);

        bool IsValidBarcode(long barcode);

        Product EditProduct(Product product);
    }
}
