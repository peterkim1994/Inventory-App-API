using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InventoryPOSApp.Core.Models.QueryModels;

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
            //bool emptyManuCode = !string.IsNullOrEmpty(product.ManufactureCode);
            var prod = _context.Products.FirstOrDefault(pr =>
               pr.Barcode == product.Barcode ||
               (
                  pr.BrandId == product.BrandId &&
                  pr.Description == product.Description &&
                  pr.ColourId == product.ColourId &&
                  pr.SizeId == product.SizeId &&
                  pr.ItemCategoryId == product.ItemCategoryId
               )
            );

            return (prod != null);
        }

        public void EditProduct(Product editedProduct)
        {
            Product product = _context.Products.Find(editedProduct.Id);
            _context.Entry(product).CurrentValues.SetValues(editedProduct);
            _context.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            return _context.Products
                              .Include(pr => pr.Brand)
                              .Include(pr => pr.Colour)
                              .Include(pr => pr.Size)
                              .Include(pr => pr.ItemCategory)
                              .FirstOrDefault(pr => pr.Id == id);
        }

        public IEnumerable<Product> GetProducts(IEnumerable<int> productIds)
        {
            return _context.Products.Where(p => productIds.Contains(p.Id))
                                     .Include(pr => pr.Brand)
                                     .Include(pr => pr.Colour)
                                     .Include(pr => pr.Size)
                                     .Include(pr => pr.ItemCategory)
                                     .AsQueryable();
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public List<ItemCategory> GetCategories()
        {
            return _context.ItemCategories.OrderBy(c => c.Value).ToList();
        }


        //inefficient but ohwell
        public async Task<List<Product>> SearchProductsAsync(InventorySearchQuery searchQuery)
        {
            var matches =
                await _context.Products
               .Include(p => p.Brand)
               .Include(p => p.ItemCategory)
               .Include(p => p.Colour)
               .Include(p => p.Size)
               .Where(p =>
                     p.StoreId == searchQuery.StoreId &&
                     (searchQuery.SearchString == null ||
                      p.Description.Contains(searchQuery.SearchString) ||
                      p.ItemCategory.Value.Contains(searchQuery.SearchString) ||
                      p.Brand.Value.Contains(searchQuery.SearchString) || 
                      p.Brand.Value.Contains(searchQuery.SearchString) || 
                      p.Colour.Value.Contains(searchQuery.SearchString)
                     ) &&
                     (searchQuery.Category == null || p.ItemCategoryId.Equals(searchQuery.Category)) &&
                     (searchQuery.Brand == null || p.BrandId.Equals(searchQuery.Brand)) &&
                     (searchQuery.Size == null || p.SizeId.Equals(searchQuery.Size)) &&
                     (searchQuery.Colour == null || p.ColourId.Equals(searchQuery.Colour))
               ).ToListAsync();

            return matches;
        }

        public List<Product> GetProducts()
        {
            return _context.Products
                    .Include(pr => pr.Brand)
                    .Include(pr => pr.Colour)
                    .Include(pr => pr.Size)
                    .Include(pr => pr.ItemCategory)
                    .ToList();

        }

        public async Task<IList<Product>> GetProductsAsync(int nTake, int nSkip, int storeId)
        {
            var products = await _context.Products
                                .Include(pr => pr.Brand)
                                .Include(pr => pr.Colour)
                                .Include(pr => pr.Size)
                                .Include(pr => pr.ItemCategory)
                                .Where(pr => pr.StoreId == storeId)
                                .Skip(nSkip)
                                .Take(nTake)
                                .ToListAsync();

            return products;
        }

        public List<Size> GetSizes()
        {
            return _context.Sizes.OrderBy(s => s.Value).ToList();
        }

        public List<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public void EditBrand(Brand brand)
        {
            var existingBrand = GetBrandByName(brand.Value);
            if (existingBrand == null)
            {
                _context.Entry<Brand>(brand).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public Brand GetBrandByName(string brandName)
        {
            return _context.Brands.FirstOrDefault(b => b.Value == brandName);
        }

        public Colour GetColourByName(string colourName)
        {
            return _context.Colours.FirstOrDefault(c => c.Value == colourName);
        }

        public ItemCategory GetItemCategoryByName(string categoryName)
        {
            return _context.ItemCategories.FirstOrDefault(c => c.Value == categoryName);
        }

        public Size GetSizeByName(string sizeName)
        {
            return _context.Sizes.FirstOrDefault(s => s.Value == sizeName);
        }

        public void EditColour(Colour colour)
        {
            _context.Entry<Colour>(colour).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void EditCategory(ItemCategory category)
        {
            _context.Entry<ItemCategory>(category).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void EditSize(Size size)
        {
            _context.Entry<Size>(size).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool IncreaseProductQty(int productId, int qty = 1)
        {
            var product = _context.Products.Find(productId);
            if (product != null || qty > 0)
            {
                product.Qty += qty;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DecreaseProductQty(int productId, int qty = 1)
        {
            var product = _context.Products.Find(productId);
            if (product != null || qty > 0)
            {
                product.Qty -= qty;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public Task<int> GetInventorySizeAsync(int storeId)
        {
            return _context.Products.CountAsync(p => p.StoreId == storeId);
        }
    }
}
