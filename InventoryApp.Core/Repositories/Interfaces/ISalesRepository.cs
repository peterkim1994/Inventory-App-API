﻿using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Repositories
{
    public interface ISalesRepository
    {
        void SaveChanges();
        Store GetStore();
        SaleInvoice GetSaleByInvoiceNumber(int invoiceNumber);
        SaleInvoice CreateNewSaleInvoice();
        void CompleteTransaction(int saleId);
        bool IsInvoiceFinalised(int saleId);
        ICollection<Product> GetProductsInTransaction(int saleId);
        ICollection<ProductSale> GetProductSalesInTransaction(int saleId);
        SaleInvoice GetPreviousSale();
        ICollection<Payment> GetSalesPayments(int saleId);
        void AddSalePayment(Payment payment);
        void AddSalesPayments(ICollection<Payment> payments);
        void AddProductSale(ProductSale productSale);
        void AddProductSales(ICollection<ProductSale> productSales);
        void UpdateSale(SaleInvoice sale);
        void CancelProductSale(ProductSale productSale);
        void DeleteSaleInvoice(int saleInvoiceId);
        SaleInvoice GetSale(int saleId);
        void CancelSale(SaleInvoice sale);
        void ClearProductSales(int saleId);
        void DeleteSalePayments(int saleId);
        void EditSalePayment(Payment payment);
        void AddRefund(Refund refund);
        Refund GetRefund(int refundId);
        IList<Refund> GetRefunds(DateTime from, DateTime to);
        IList<SaleInvoice> GetSales(DateTime from, DateTime to);
        IList<SaleInvoice> GetInvoicePayments(DateTime from, DateTime to);

        IList<ProductSale> GetProductSales(ICollection<int> ids);

        void UpdateProductSales(IList<ProductSale> productSales);
    }
}
