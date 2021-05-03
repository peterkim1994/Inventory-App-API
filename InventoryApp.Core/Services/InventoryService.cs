using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
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
            return true;
        }

        public Product AddProduct(Product product)
        {
            _repo.AddNewProduct(product);
            _repo.SaveChanges();
            return product;
        }

        public ItemCategory AddCategory(ItemCategory category)
        {
            category.Value = TextProcessor.ToPronounCasing(category.Value);
            if (_repo.ContainsAtt(category))
            {
                return null;
            }
            _repo.AddItemCategory(category);
            return category;
        }


    }
}
