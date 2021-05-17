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

        Promotion GetPromotionByName(string promotionName);

        ProductPromotion GetProductPromotion(int productId, int promotionId);

        IList<Promotion> GetPromotionsByDateRange(DateTime start, DateTime end);

        IList<Promotion> GetActivePromotions();

        void RemoveProductPromotion(ProductPromotion productPromotion);

        IList<Product> GetPromotionsProducts(int promotionId);

        Promotion GetPromotion(int id);

        SaleInvoice GetSaleByInvoiceNumber(int invoiceNumber);

        SaleInvoice CreateNewSaleInvoice();

        void AddProductToTransaction(int productId, SaleInvoice invoice);

        void CompleteTransaction(int saleId);

        bool IsInvoiceFinalised(int saleId);

        ICollection<Product> GetProductsInTransaction(int saleId);

        SaleInvoice GetCurerntSale();

        ICollection<Payment> GetSalesPayments(int saleId);

        void AddSalePayment(Payment payment);

        void AddProductSale(ProductSale productSale);

        void DeleteProductSale(ProductSale productSale);

        void DeleteSaleInvoice(int saleInvoiceId);




    }
}
