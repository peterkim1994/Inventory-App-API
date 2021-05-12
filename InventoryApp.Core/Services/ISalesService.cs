using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Services
{
    public interface ISalesService
    {

        bool AddPromotion(Promotion promotion);

        bool AddProductToPromotion(int product, int promotionId);

        Promotion GetPromotion(int id);

    }
}
