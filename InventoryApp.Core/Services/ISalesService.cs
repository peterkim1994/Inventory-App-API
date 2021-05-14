using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Services
{
    public interface ISalesService
    {

        bool AddPromotion(Promotion promotion);

        bool AddProductToPromotion(int productId, int promotionId);

        Promotion EditPromotion(Promotion promotion);

        bool RemoveProductPromotion(int productId, int PromotionId);

        IList<Promotion> GetActivePromotions();

    }
}
