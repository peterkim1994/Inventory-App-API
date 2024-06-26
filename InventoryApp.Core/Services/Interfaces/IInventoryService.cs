﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;
using InventoryPOSApp.Core.Models.QueryModels;

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

        Task<IList<Product>> GetAllProductsAsync(AllProductQueryModel productQuery);

        List<Product> GetProducts(List<int> productIds);

        bool BarcodeIsAvailable(long barcode);

        Product EditProduct(Product product);
    }
}
