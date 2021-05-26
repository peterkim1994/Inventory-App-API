using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Services.Interfaces
{
    public interface IPromotionsService
    {
        bool AddPromotion(Promotion promotion);

        bool AddProductToPromotion(int productId, int promotionId);

        void EditPromotion(Promotion promotion);

        bool RemoveProductPromotion(int productId, int PromotionId);

        IList<Promotion> GetActivePromotions();

        ICollection<ProductPromotion> GetProductPromotions();

        void DeletePromotion(int promotionId);

        IList<Product> GetPromotionsProducts(int promotionId);

        Promotion GetPromotion(int promotionId);
    }
}
