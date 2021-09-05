using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Services
{
    public class StoreManagementService : IStoreManagementService
    {
        private ISalesService _saleService { get; set; }

        private IInventoryService _inventoryService { get; set; }

        private DBContext _context { get; set; }

        public StoreManagementService
        (
            ISalesService salesService,
            IInventoryService inventoryService,
            DBContext context         
        )
        {
            _saleService = salesService;
            _context = context;
            _inventoryService = inventoryService;
        }

        public IList<SaleInvoice> GetPreviousSales(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public ProductSale GetProductsSold(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public SaleInvoice GetRefunds(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalRevenue(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public bool VoidSales(IList<SaleInvoice> sale)
        {
            throw new NotImplementedException();
        }
    }
}
