using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;

namespace InventoryPOSApp.Core.Repositories
{
    public interface IInventoryRepo
    {
        Product GetProduct(int id);

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

        void EditBrand(Brand brand);

        void EditColour(Colour colour);

        void EditCategory(ItemCategory category);

        void EditSize(Size size);

        List<Brand> GetBrands();

        bool ContainsAtt<T>(T newAtt) where T : ProductAttribute;

        List<T> GetProductAttributes<T>() where T : ProductAttribute;

        Product GetProductByBarcode(long code);
        List<Product> SearchProducts(string searchWord);
        void SaveChanges();

        Brand GetBrandByName(string brandName);

        Colour GetColourByName(string colourName);

        ItemCategory GetItemCategoryByName(string categoryName);

        Size GetSizeByName(string sizeName);

        List<Product> GetProducts(List<int> productIds);

        bool IncreaseProductQty(int productId, int qty);

        bool DecreaseProductQty(int productId, int qty);

       
    }
}
