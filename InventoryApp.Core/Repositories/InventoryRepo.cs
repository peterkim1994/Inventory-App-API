using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;
using InventoryPOS.DataStore.Models.Interfaces;
using System.Linq;

namespace InventoryPOSApp.Core.Repositories
{
    public class InventoryRepo : IInventoryRepo
    {
        private DBContext _context { get; }

        public InventoryRepo(DBContext context)
        {
            _context = context;
        }

        public void AddColour(Colour colour)
        {
            _context.Add(colour);
        }

        public List<Colour> GetColours()
        {
            return _context.Colours.ToList();
        }

        public void AddBrand(Brand brand)
        {
            _context.Add(brand);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void AddItemCategory(ItemCategory category)
        {
            _context.Add(category);
        }

        public void AddNewProduct(Product product)
        {
            _context.Add(product);
        }

        public void AddSize(Size size)
        {
            _context.Add(size);
        }

        public Product GetProductByBarcode(long code)
        {
            var product = _context.Products.First(product => product.Barcode == code);
            return product;
        }

        public List<T> GetProductAttributes<T>() where T : ProductAttribute
        {          
            List<T> x = _context.Set<T>().ToList();            
            foreach( var ss in x)
            {
                System.Diagnostics.Debug.WriteLine("\n\n " + ss.Id + "\n");
            }
            return x;
        }

        public bool ContainsAtt<T>(T newAtt) where T : ProductAttribute
        {               
            var set = _context.Set<T>();
            var rows = set.ToList();
            var attribute = from at in rows
                    where at.Value == newAtt.Value
                    select at;
            return (attribute.Count() == 1); 
        }

        public bool ContainsProduct(Product product)
        {
            var prod = _context.Products.First(pr => 
                pr.ManufactureCode == product.ManufactureCode
                && pr.Name == product.Name 
                && pr.Brand == product.Brand
                && pr.Description == product.Description
                && pr.Colour == product.Colour
                && pr.Size == product.Size
                && pr.ItemCategory == product.ItemCategory
            );
            if (prod == null)
                return false;
            return true;
        }

        public void EditProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }


        public List<ItemCategory> GetCategories()
        {
            return _context.ItemCategories.ToList();
        }


        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.ToList();
        }

        public List<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }
    }
}
