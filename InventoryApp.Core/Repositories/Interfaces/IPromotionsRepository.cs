using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Repositories
{
    public interface IPromotionsRepository
    {
        Dictionary<int, IList<Promotion>> GetProductActivePromotions();

        IList<Promotion> GetPromotionsByDate(DateTime rangeStart, DateTime rangeEnd);

        void AddPromotion(Promotion promotion);

        void AddProductPromotion(ProductPromotion productPromotion);

        void EditPromotion(Promotion promotion);

        void ClearPromotionProducts(int promotionId);

        Promotion GetPromotionByName(string promotionName);

        ProductPromotion GetProductPromotion(int productId, int promotionId);

        IList<Promotion> GetPromotionsByDateRange(DateTime start, DateTime end);

        IList<Promotion> GetActivePromotions();

        void RemoveProductPromotion(ProductPromotion productPromotion);

        IList<Product> GetPromotionsProducts(int promotionId);

        Promotion GetPromotion(int id);
    }
}
