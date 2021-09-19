using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;

namespace InventoryPOSApp.Core.Services
{
    public interface IStoreManagementService
    {
        IList<SaleInvoice> GetPreviousSales(DateTime from, DateTime to);

        SalesReportDto GetSalesReport(DateTime from, DateTime to);

        bool VoidSale(int saleId, int productSaleId);

        ProductSale GetProductsSold(DateTime from, DateTime to);        
    }
}
