using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Daos;
using Microsoft.EntityFrameworkCore;
using InventoryPOSApp.Core.Dtos;


namespace InventoryPOSApp.Core.Repositories
{
    public class SalesRepository : ISalesRepository
    {

        private readonly DBContext _context;

        public SalesRepository(DBContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Store GetStore()
        {
            return _context.Store.Find(1);
        }

        public SaleInvoice GetSaleByInvoiceNumber(int invoiceNumber)
        {
            return _context.SaleInvoices
                .Include(s => s.ProductSales)
                .Include(s => s.Payments)
                .Include(s => s.Refunds)
                .FirstOrDefault(s => s.Id == invoiceNumber);
        }

        public SaleInvoice CreateNewSaleInvoice()
        {
            SaleInvoice newSale = new SaleInvoice { InvoiceDate = DateTime.Now };
            _context.SaleInvoices.Add(newSale);           
            //      Store store = new Store { StoreName = "Procamp", Address = "Hamilton", GstNum = "123-234432-332", Contact = "07-801-2345" };
            //     _context.Store.Add(store);
            _context.SaveChanges();
            return newSale;
        }

        public ICollection<Product> GetProductsInTransaction(int saleId)
        {
            var products = from sale in _context.ProductSales
                           where sale.SaleInvoiceId == saleId
                           join p in _context.Products
                           on sale.ProductId equals p.Id
                           select p;
            return products.ToList();
        }

        public ICollection<ProductSale> GetProductSalesInTransaction(int saleId)
        {
            var productSales = _context.ProductSales
                                    .Include(ps => ps.Product)
                                    .Select(ps => ps.SaleInvoiceId == saleId);
            return (ICollection<ProductSale>)productSales;
        }

        public SaleInvoice GetPreviousSale()
        {
            return _context.SaleInvoices.Last();
        }

        public void AddProductSales(ICollection<ProductSale> productSales)
        {
            _context.ProductSales.AddRange(productSales.ToList());
            var x = _context.ProductSales;
            _context.SaveChanges();
        }

        public void CancelSale(SaleInvoice sale)
        {
            sale.Canceled = true;
            _context.Entry(sale).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddProductSale(ProductSale productSale)
        {
            _context.ProductSales.Add(productSale);
            _context.SaveChanges();
        }

        public void CancelProductSale(ProductSale productSale)
        {
            ProductSale product = _context.ProductSales.Find(productSale.Id);            

            if (product != null && product.Canceled == false)
            {
                product.Canceled = true;
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void ClearProductSales(int saleId)
        {
            var productSales = from ps in _context.ProductSales
                               where ps.SaleInvoiceId == saleId
                               select ps;
            if (productSales.Count() == 0)
                return;
            else {
                _context.ProductSales.RemoveRange(productSales);
                SaveChanges();
            }
        }

        public void UpdateSale(SaleInvoice sale)
        {
            _context.Entry<SaleInvoice>(sale).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public ICollection<Payment> GetSalesPayments(int saleId)
        {
            var payments = from sale in _context.SaleInvoices
                           where sale.Id == saleId
                           join payment in _context.Payments
                           on sale.Id equals payment.SaleInvoiceId
                           select payment;

            var salePayments = from sale in _context.SaleInvoices
                               from payment in _context.Payments
                               where sale.Id == saleId && sale.Id == payment.SaleInvoiceId
                               join paymentMethod in _context.PaymentMethods
                                    on payment.PaymentMethodId equals paymentMethod.Id
                               select new Payment { PaymentMethod = paymentMethod, Amount = payment.Amount };

            return salePayments.ToList();

        }
        public bool RemovePayment(Payment payment)
        {
            Payment pay = _context.Payments
                                    .FirstOrDefault(p =>
                                        p.SaleInvoiceId == payment.SaleInvoiceId &&
                                        p.PaymentMethodId == payment.PaymentMethodId &&
                                        p.Amount == payment.Amount
                                    );
            if (pay == null) return false;

            _context.Payments.Remove(pay);
            _context.SaveChanges();
            return true;
        }


        public bool IsInvoiceFinalised(int saleId)
        {
            var sale = _context.SaleInvoices.Find(saleId);
            return (sale.Finalised == true);
        }

        public void CompleteTransaction(int saleInvoiceId)
        {
            SaleInvoice sale = _context.SaleInvoices.Find(saleInvoiceId);
            sale.Finalised = true;
            _context.Entry(sale).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public void DeleteSaleInvoice(int saleInvoiceId)
        {
            SaleInvoice sale = _context.SaleInvoices.Find(saleInvoiceId);
            //Delete ProductSales?
            _context.SaleInvoices.Remove(sale);
            _context.SaveChanges();
        }

        public SaleInvoice GetSale(int saleId)
        {
            var sale = _context.SaleInvoices
                .Include(s => s.ProductSales)
                .Include(s => s.Payments)
                .FirstOrDefault(s => s.Id == saleId);
            if (sale == null)
                return sale;
            foreach (var payment in sale.Payments)
            {
                _context.Entry(payment).Reference(p => p.PaymentMethod).Load();
            }
            return sale;

        }

        public void DeleteSalePayments(int saleId)
        {
            var payments = from p in _context.Payments
                           where p.SaleInvoiceId == saleId
                           select p;
            _context.RemoveRange(payments);
            SaveChanges();
        }

        public void EditSalePayment(Payment payment)
        {
            //check this works
            var p = _context.Entry<Payment>(payment).State = EntityState.Modified;
            SaveChanges();
        }

        public void AddSalePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public void AddSalesPayments(ICollection<Payment> payments)
        {
            _context.Payments.AddRange(payments);
            _context.SaveChanges();
        }


        public void AddRefund(Refund refund)
        {
            _context.Refunds.Add(refund);
            _context.SaveChanges();
        }

        public Refund GetRefund(int refundId)
        {
           return _context.Refunds.Find(refundId);
        }

        public IList<SaleInvoice> GetSales(DateTime from, DateTime to)
        {
            from = from == null ? DateTime.MinValue : from;
            to = to == null ? DateTime.MaxValue : to;
            var saleInvoices = _context.SaleInvoices.Include(s => s.ProductSales.Where(p => p.Canceled == false))
                                                    .ThenInclude(p => p.Product)                                                   
                                                    .Include(s => s.Refunds)
                                                    .Include(s => s.Payments)
                                                    .ThenInclude(p=> p.PaymentMethod)
                                                    .Where(s => s.Finalised == true &&
                                                                 s.InvoiceDate >= from &&
                                                                 s.InvoiceDate <= to
                                                            )
                                                    .OrderBy(s => s.Id);
            //ohwell
            var products = saleInvoices.SelectMany(s => s.ProductSales).Select(p => p.Product).ToList();
          
            foreach(var p in products)
            {                 
               _context.Entry<Product>(p).Reference(pr => pr.Brand).Load();
               _context.Entry<Product>(p).Reference(pr => pr.ItemCategory).Load();
               _context.Entry<Product>(p).Reference(pr => pr.Size).Load();
               _context.Entry<Product>(p).Reference(pr => pr.Colour).Load();
            }         

            return saleInvoices.ToList();
        }

        public IList<SaleInvoice> GetInvoicePayments(DateTime from, DateTime to)
        {
            from = from == null ? DateTime.MinValue : from;
            to = to == null ? DateTime.MaxValue : to;
            var saleInvoices = _context.SaleInvoices.Include(s => s.ProductSales.Where(p=>p.Canceled == false))                                                   
                                                    .Include(s => s.Refunds)
                                                    .Include(s => s.Payments)
                                                    .ThenInclude(p => p.PaymentMethod)
                                                    .Where(s => s.Finalised == true &&
                                                                 s.InvoiceDate >= from &&
                                                                 s.InvoiceDate <= to
                                                            ).ToList();          

            return saleInvoices;
        }

        public IList<Refund> GetRefunds(DateTime from, DateTime to)
        {
            from = from == null ? DateTime.MinValue : from;
            to = to == null ? DateTime.MaxValue : to;
            var refunds = _context.Refunds.Where(r =>  r.RefundDate >= from &&
                                                       r.RefundDate <= to
                                                );
            return refunds.ToList();
        }

        public IList<ProductSale> GetProductSales(ICollection<int> ids)
        {
           return _context.ProductSales.Where(ps => ids.Contains(ps.Id)).ToList();
        }

        public void UpdateProductSales(IList<ProductSale> productSales)
        {
            _context.UpdateRange(productSales);
            _context.SaveChanges();
        }
    }
}
