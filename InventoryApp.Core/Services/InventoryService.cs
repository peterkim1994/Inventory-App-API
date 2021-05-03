using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
using InventoryPOS.DataStore.Models.Interfaces;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Utils;

namespace InventoryPOSApp.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepo _repo { get; set; }
        public InventoryService(IInventoryRepo inventoryRepo)
        {
            _repo = inventoryRepo;
        }

        public bool AddColour(Colour colour)
        {
            colour.Value = TextProcessor.ToPronounCasing(colour.Value);
            if (_repo.ContainsAtt(colour))
            {
                return false;
            }
            _repo.AddColour(colour);
            _repo.SaveChanges();
            return true;
        }

        public List<IEnumerable<ProductAttribute>> GetProductAttributes()   
        {
            List<IEnumerable<ProductAttribute>> allAttributes= new List<IEnumerable<ProductAttribute>>();
            IEnumerable<ProductAttribute> colours =_repo.GetProductAttributes<Colour>();
            IEnumerable<ProductAttribute> brands = _repo.GetProductAttributes<Brand>();
            IEnumerable<ProductAttribute> categories = _repo.GetProductAttributes<ItemCategory>();
            IEnumerable<ProductAttribute> sizes = _repo.GetProductAttributes<Size>();
            allAttributes.Add(colours);
            allAttributes.Add(sizes);
            allAttributes.Add(categories);
            allAttributes.Add(sizes);
            return allAttributes;
        }


        public bool AddItemCategory(ItemCategory category)
        {
            category.Value = TextProcessor.ToPronounCasing(category.Value);
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
            size.Value = size.Value.ToUpper().Trim();
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
            brand.Value = TextProcessor.ToPronounCasing(brand.Value);
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
            string code = string.Empty;
            for (int i = 0; i < 12; i++)
                code = String.Concat(code, random.Next(10).ToString());         
            return long.Parse(code);
        }

        public bool AddProduct(Product product)
        {
            if (_repo.ContainsProduct(product))            
                return false;            
            else
            {
                if(product.Barcode == 0)
                {
                    long newBarcode = GenerateBarcode();
                    while (_repo.GetProductByBarcode(newBarcode) != null)
                    {
                        newBarcode = GenerateBarcode();
                    }
                    product.Barcode = newBarcode;          
                }
                _repo.AddNewProduct(product);
                _repo.SaveChanges();
                return true;
            }
        }
    }
}
