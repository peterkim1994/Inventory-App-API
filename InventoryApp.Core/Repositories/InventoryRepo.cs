using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Repositories
{
    public class InventoryRepo : IInventoryRepo
    {
        private DBContext _context { get; }

        public InventoryRepo(DBContext context)
        {
            _context = context;
        }

        public Colour AddColour(Colour colour)
        {
            _context.Add(colour);
            return colour;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ItemCategory AddItemCategory(ItemCategory category)
        {
            _context.Add(category);
            return category;
        }

        public void AddNewProduct(Product product)
        {
            _context.Add(product);
        }
    }
}
