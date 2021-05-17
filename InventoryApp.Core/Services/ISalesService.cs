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

        void EditPromotion(Promotion promotion);

        bool RemoveProductPromotion(int productId, int PromotionId);

        IList<Promotion> GetActivePromotions();

        ICollection<ProductPromotion> GetProductPromotions();

        IList<Promotion> CheckEligibalePromotions(IList<Product> products);

        int CalculateTotal(int saleInvoiceId);

        Payment ProcessPayement(Payment payment, SaleInvoice sale);

        bool ValidateSalePayments(SaleInvoice sale);

        SaleInvoice StartSaleTransaction();

        bool CancelPrevSale();

        bool AddProductToSale(int SaleId, int productId);

    }
}
