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

        public bool ContainsAtt<T>(T newAtt) where T : ProductAttribute
        {               
            var set = _context.Set<T>();
            var rows = set.ToList();
            var attribute = from at in rows
                    where at.Value == newAtt.Value
                    select at;
            return (attribute.Count() == 1); 
        }
        
    }
}
