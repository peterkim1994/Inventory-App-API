using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Repositories
{
    public class InventoryRepo
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

        }
    }
}
