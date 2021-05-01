using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Repositories;

namespace InventoryPOSApp.Core.Services
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepo _repo { get; set; }
        public InventoryService(IInventoryRepo inventoryRepo)
        {
            _repo = inventoryRepo;
        }

        public Colour AddColour(Colour colour)
        {
            _repo.AddColour(colour);
            return colour;
        }
    }
}
