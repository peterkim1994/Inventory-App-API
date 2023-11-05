using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Utils;
using System.Threading.Tasks;
using InventoryPOSApp.Core.Models.QueryModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using InventoryPOSApp.Core.Models.ResponseModels;

namespace InventoryPOSApp.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepo _repo { get; set; }

        public InventoryService(IInventoryRepo inventoryRepo)
        {
            _repo = inventoryRepo;
        }

        public async Task<(IList<Product>, int)> GetInventoryCatelogAsync(InventoryCatelogRequest inventoryCatelogRequest)
        {
            var products = await GetInventorySlice(inventoryCatelogRequest);
            int numItems = await _repo.GetInventorySizeAsync(inventoryCatelogRequest.StoreId);

            return (products, numItems);
        }

        public async Task<IList<Product>> GetInventorySlice(InventoryCatelogRequest productQuery)
        {
            int skipAmmount = (productQuery.StartPage - 1) * productQuery.NumItemsPerPage;
            int numPagesToLoad = productQuery.PageLoadBuffer - productQuery.StartPage - 1;
            int numbOfProductsToLoad = productQuery.NumItemsPerPage * numPagesToLoad;

            return await _repo.GetProductsAsync(numbOfProductsToLoad, skipAmmount, productQuery.StoreId);
        }

        public List<Product> GetAllProducts()
        {
            return _repo.GetProducts();
        }

        public bool AddColour(Colour colour)
        {
            colour.Value = TextProcessor.ToTitleCase(colour.Value);
            if (_repo.ContainsAtt(colour))
            {
                return false;
            }

            _repo.AddColour(colour);
            _repo.SaveChanges();

            return true;
        }

        [Obsolete("create async method pls")]
        public List<IEnumerable<ProductAttribute>> GetProductAttributes()   
        {
            List<IEnumerable<ProductAttribute>> allAttributes = new List<IEnumerable<ProductAttribute>>();

            IEnumerable<ProductAttribute> colours =_repo.GetProductAttributes<Colour>();
            IEnumerable<ProductAttribute> brands = _repo.GetProductAttributes<Brand>();
            IEnumerable<ProductAttribute> categories = _repo.GetProductAttributes<ItemCategory>();

            SizeComparer sc = new SizeComparer();
            List<Size> sizeList =  _repo.GetProductAttributes<Size>();
            sizeList.Sort(sc); 

            IEnumerable<ProductAttribute> sizes = sizeList;

            allAttributes.Add(colours); 
            allAttributes.Add(brands);
            allAttributes.Add(categories);
            allAttributes.Add(sizes);

            return allAttributes;
        }

        public bool AddItemCategory(ItemCategory category)
        {
            category.Value = TextProcessor.ToTitleCase(category.Value);
            if (_repo.ContainsAtt(category))
            {
                return false;
            }

            _repo.AddItemCategory(category);
            _repo.SaveChanges();

            return true;
        }

        public bool AddSize(Size size)
        {
            size.Value = size.Value.ToUpper();

            if (_repo.ContainsAtt(size))
            {
                return false;
            }

            _repo.AddSize(size);
            _repo.SaveChanges();

            return true;
        }

        public bool AddBrand(Brand brand)
        {
            brand.Value = TextProcessor.ToTitleCase(brand.Value);

            if (_repo.ContainsAtt(brand))
            {
                return false;
            }

            _repo.AddBrand(brand);
            _repo.SaveChanges();

            return true;
        }

        public long GenerateBarcode()
        {
            Random random = new Random();
            long newBarcode;
            do
            {
                string code = string.Empty;
                for (int i = 0; i < 9; i++)
                    code = String.Concat(code, random.Next(10).ToString());
                newBarcode = long.Parse(code);
            } while (_repo.GetProductByBarcode(newBarcode) != null && newBarcode < 500);
            return newBarcode;
        }

        //TERRIBLE LOGIC FLOW
        public bool AddProduct(Product product)
        {
            if (_repo.ContainsProduct(product))            
                return false;            
            else
            {
                if(product.Barcode == 0)                                   
                    product.Barcode = GenerateBarcode();                

                if(BarcodeIsAvailable(product.Barcode))
                {                    
                    _repo.AddNewProduct(product);
                    _repo.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Product EditProduct(Product product)
        {
            if(product.Barcode == 0)
            {
                product.Barcode = GenerateBarcode();
            }

            _repo.EditProduct(product);
            
            return product;
        }


        public bool BarcodeIsAvailable(long barcode)
        {
            return _repo.GetProductByBarcode(barcode) == null;           
        }

        public bool EditBrand(Brand brand)
        {
            // note for later: validation needs to improve for product attributes,,
            if (ProductAttributeNotNull(brand) == false)
            {
                return false;
            }

            brand.Value = TextProcessor.ToPronounCasing(brand.Value);
            var existingBrand = _repo.GetBrandByName(brand.Value);

            if (existingBrand == null)
            {
                _repo.EditBrand(brand);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditColour(Colour colour)
        {
            if(ProductAttributeNotNull(colour) == false)
            {
                return false;
            }
            colour.Value = TextProcessor.ToPronounCasing(colour.Value);
            var existingBrand = _repo.GetColourByName(colour.Value);
            if (existingBrand == null)
            {
                _repo.EditColour(colour);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditCategory(ItemCategory category)
        {
            if (category == null || String.IsNullOrEmpty(category.Value))
                return false;
            category.Value = TextProcessor.ToPronounCasing(category.Value);
            var existingCategory = _repo.GetItemCategoryByName(category.Value);
            if (existingCategory == null)
            {
                _repo.EditCategory(category);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditSize(Size size)
        {
            if (size == null || String.IsNullOrEmpty(size.Value))
                return false;
            size.Value = TextProcessor.ToPronounCasing(size.Value);
            var existingSize = _repo.GetSizeByName(size.Value);
            if (existingSize == null)
            {
                _repo.EditSize(size);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> GetProductsWithAttributes(IEnumerable<int> productIds)
        {
            var productList = _repo.GetProducts(productIds).ToList();

            return productList;
        }

        
        //helper func to check if product attribute is null or has an empty value
        public bool ProductAttributeNotNull(ProductAttribute att)
        {
            if (att == null || String.IsNullOrEmpty(att.Value))
                return false;
            return true;
        }
    }
}
