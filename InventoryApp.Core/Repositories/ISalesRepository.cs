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

        void AddPromotion(Promotion promotion);

        void EditPromotion(Promotion promotion);

        Promotion CheckPromotionEligibility(IList<Product> productsToBeSold);

        void AddProductsToTransaction(IList<Product> products, SaleInvoice invoice);

        bool CompleteTransaction(SaleInvoice invoice);


    }
}
