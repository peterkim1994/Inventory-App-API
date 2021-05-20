using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Repositories
{
    public interface ISalesRepository
    {
        void SaveChanges();

        SaleInvoice GetSaleByInvoiceNumber(int invoiceNumber);

        SaleInvoice CreateNewSaleInvoice();

        void AddProductToTransaction(int productId, SaleInvoice invoice);

        void CompleteTransaction(int saleId);

        bool IsInvoiceFinalised(int saleId);

        ICollection<Product> GetProductsInTransaction(int saleId);

        SaleInvoice GetPreviousSale();

        ICollection<Payment> GetSalesPayments(int saleId);

        void AddSalePayment(Payment payment);

        void AddProductSale(ProductSale productSale);

        void DeleteProductSale(ProductSale productSale);

        void DeleteSaleInvoice(int saleInvoiceId);

        SaleInvoice GetSale(int saleId);

        void ClearProductSales(int saleId);

        void DeleteSalePayments(int saleId);

        void EditSalePayment(Payment payment);

        
    }
}
