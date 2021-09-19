using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOSApp.Core.Services
{
    public class StoreManagementService : IStoreManagementService
    {
        private ISalesService _saleService { get; set; }

        private IInventoryService _inventoryService { get; set; }

        private ISalesRepository _saleRepo { get; }

        private DBContext _context { get; set; }

        public StoreManagementService
        (
            ISalesService salesService,
            IInventoryService inventoryService,
            ISalesRepository saleRepo,
            DBContext context
        )
        {
            _saleService = salesService;
            _saleRepo = saleRepo;
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

        public SalesReportDto GetSalesReport(DateTime from, DateTime to)
        {
            SalesReportDto report = new SalesReportDto
            {
                From = from.ToString(),
                To = to.ToString(),
                CashAmount = 0.00m,
                EftposAmount = 0.00m,
                AfterPayAmount = 0.00m,
                StoreCreditAmount = 0.00m,
                TotalRefunds = 0.00m,
                TotalAmount = 0.00m,
                NetTotal = 0.00m,
            };

            var invoices = _saleRepo.GetInvoicePayments(from, to);
            if (invoices == null || invoices.Count() == 0)
            {
                return report;
            }

            //   IEnumerable<IGrouping<int, int>> payments = invoices.SelectMany(si => si.Payments)
            //                       .GroupBy(p => p.PaymentMethodId, p => p.Amount);                
            var payments = invoices.SelectMany(si => si.Payments).ToList();

            var cash = payments.Where(p => p.PaymentMethodId == 1).Sum(p => p.Amount);
            var eftpos = payments.Where(p => p.PaymentMethodId == 2).Sum(p => p.Amount);
            var afterPay = payments.Where(p => p.PaymentMethodId == 3).Sum(p => p.Amount);
            var storeCredit = payments.Where(p => p.PaymentMethodId == 4).Sum(p => p.Amount);
            var refund = invoices.SelectMany(i => i.Refunds).Sum(r => r.Amount);

            report.CashAmount = ToDecimal2dp(cash);
            report.EftposAmount = ToDecimal2dp(eftpos);
            report.AfterPayAmount = ToDecimal2dp(afterPay);
            report.StoreCreditAmount = ToDecimal2dp(storeCredit);
            report.TotalRefunds = ToDecimal2dp(refund);

            report.TotalAmount = Decimal.Round((report.CashAmount + report.EftposAmount + report.AfterPayAmount),2);
            report.NetTotal = Decimal.Round((report.TotalAmount - report.TotalRefunds), 2);

            return report;
        }

        public bool VoidSale(int saleId, int productSaleId)
        {
            var sale =  _context.SaleInvoices
                                .Include(s => s.ProductSales)
                                .Include(s => s.Payments)
                                .FirstOrDefault(s => s.Id == saleId);

            if (sale == null || sale.Finalised == false || sale.Canceled == true)
            {
                return false;
            }

            if(sale.ProductSales.Count() < 2)
            {
                return false;
            }

            bool nonCashPayments = sale.Payments.Any(p => p.SaleInvoiceId == saleId && p.PaymentMethodId != 1);

            var productSold = sale.ProductSales.FirstOrDefault(ps => ps.Id == productSaleId);

            if (nonCashPayments || productSold == null || productSold.PriceSold >= sale.Payments.Sum(p=>p.Amount))
            {
                return false;
            }

            var payment = sale.Payments.SingleOrDefault(p => p.PaymentMethodId == 1);
            payment.Amount -= productSold.PriceSold;
            if (payment.Amount > 100)
            {
                _saleRepo.CancelProductSale(new ProductSale { Id = productSold.Id });
                
                _saleRepo.EditSalePayment(payment);
                return true;
            }

            return false;
        }

        private Decimal ToDecimal2dp(int amount)
        {
            if (amount == 0)
                return 0.00m;

            Decimal value = (amount / 100);
            return Decimal.Round(value, 2);
        }
    }
}
