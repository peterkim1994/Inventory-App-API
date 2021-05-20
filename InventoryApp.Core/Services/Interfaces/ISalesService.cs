using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Services
{
    public interface ISalesService
    {

        IList<ProductSale> ApplyPromotions(int saleId, List<Product> products);

        int CalculateTotal(int saleInvoiceId);

        Payment ProcessPayement(Payment payment, SaleInvoice sale);

        bool ValidateSalePayments(SaleInvoice sale);

        SaleInvoice StartSaleTransaction();

        void CancelSale();

        bool AddProductToSale(int SaleId, int productId);

    }
}
