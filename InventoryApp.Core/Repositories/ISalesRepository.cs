using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Repositories
{
    public interface ISalesRepository
    {
        void SaveChanges();

        IList<Promotion> GetCurrentPromotions();

        IList<Promotion> GetPromotionsByDate(DateTime rangeStart, DateTime rangeEnd);

        void AddPromotion(Promotion promotion);

        void AddProductPromotion(ProductPromotion productPromotion);

        void EditPromotion(Promotion promotion);

      //  Promotion CheckPromotionEligibility(IList<Product> productsToBeSold);

        void AddProductsToTransaction(IList<Product> products, SaleInvoice invoice);

        bool CompleteTransaction(SaleInvoice invoice);

        bool IsInvoiceFinalised(SaleInvoice invoice);

        Promotion GetPromotionByName(string promotionName);

        ProductPromotion GetProductPromotion(int productId, int promotionId);

        IList<Promotion> GetActivePromotions();

        IList<Promotion> GetInactivePromotions();

        void RemoveProductPromotion(int productId, int promotionId);
    }
}
