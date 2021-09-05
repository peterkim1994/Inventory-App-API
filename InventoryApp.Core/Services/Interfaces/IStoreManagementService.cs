using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Services
{
    public interface IStoreManagementService
    {
        IList<SaleInvoice> GetPreviousSales(DateTime from, DateTime to);

        Decimal GetTotalRevenue(DateTime from, DateTime to);

        SaleInvoice GetRefunds(DateTime from, DateTime to);

        bool VoidSales(IList<SaleInvoice> sale);

        ProductSale GetProductsSold(DateTime from, DateTime to);
        
    }
}
