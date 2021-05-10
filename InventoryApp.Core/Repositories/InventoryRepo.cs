using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;
using InventoryPOS.DataStore.Models.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var product = _context.Products.FirstOrDefault(prod => prod.Barcode == code);
            return product;
        }

        public List<T> GetProductAttributes<T>() where T : ProductAttribute
        {          
            List<T> x = _context.Set<T>().ToList();            
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
            var prod =  _context.Products.Where(pr =>
                pr.ManufactureCode == product.ManufactureCode
                && pr.BrandId == product.BrandId
                && pr.Description == product.Description
                && pr.ColourId == product.ColourId
                && pr.SizeId == product.SizeId
                && pr.ItemCategoryId == product.ItemCategoryId
            );
            bool s = prod.Count() > 0;
            return (s);
        }

        public void EditProduct(Product editedProduct)
        {
            //  var product = _context.Products.First(p => p.Id == editedProduct.Id);
            //  product = editedProduct;
            Product product = _context.Products.Find(editedProduct.Id);       
            _context.Entry(product).CurrentValues.SetValues(editedProduct);
          //  State = EntityState.Modified;
            //product = editedProduct;
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }


        public List<ItemCategory> GetCategories()
        {
            return _context.ItemCategories.OrderBy(c => c.Value).ToList();
        }


        public List<Product> GetProducts()
        {
            return _context.Products
                .Include(pr => pr.Brand)
                .Include(pr => pr.Colour)
                .Include(pr => pr.Size)
                .Include(pr => pr.ItemCategory)
                .ToList();
          //  var prods = _context.Products.Include()
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.OrderBy(s => s.Value).ToList();
        }

        public List<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }
    }
}
