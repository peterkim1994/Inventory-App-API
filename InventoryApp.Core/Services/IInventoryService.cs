using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Services
{
    public interface IInventoryService
    {
        Colour AddColour(Colour colour);
    }
}
