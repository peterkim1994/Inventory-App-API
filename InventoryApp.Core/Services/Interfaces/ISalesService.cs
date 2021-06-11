using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Services
{
    public interface ISalesService
    {
        Store GetStore();
        IList<ProductSale> ApplyPromotionsToSale(int saleId, List<Product> products);

        SaleInvoice GetSale(int saleId);

        int ProcessSaleTotalAmount(ICollection<ProductSale> productSales);

        bool ProcessPayments(ICollection<Payment> payments);

        bool ValidatePaymentAmount(int totalOwing, ICollection<Payment> payments);

        SaleInvoice StartNewSaleTransaction();

        bool CancelSale(int saleId);

        void ProcessProductSales(SaleInvoice sale, IList<ProductSale> productSales);

        
    //    ProductSale ProcessProductSale(int saleId, Product product, Promotion promotion, int sellingPrice);

 
    }
}
