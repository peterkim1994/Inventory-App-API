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

        List<Colour> GetColours();  

        void AddItemCategory(ItemCategory category);

        List<ItemCategory> GetCategories();

        bool ContainsProduct(Product product);

        List<Product> GetProducts();
         
        void EditProduct(Product product);

        void DeleteProduct(Product product);

        void AddNewProduct(Product product);

        void AddSize(Size size);

        List<Size> GetSizes();

        void AddBrand(Brand brand);

        List<Brand> GetBrands();

        bool ContainsAtt<T>(T newAtt) where T : ProductAttribute;

        List<T> GetProductAttributes<T>() where T : ProductAttribute;

        Product GetProductByBarcode(long code);

      //  List<Product> GetAllProducts();
        void SaveChanges();
    }
}
