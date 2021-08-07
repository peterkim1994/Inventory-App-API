using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Comparers;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Utils;

namespace InventoryPOSApp.Core.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _salesRepo;
        private readonly IPromotionsRepository _promoRepo;
        private readonly IInventoryRepo _inventoryRepo;

        public SalesService
        (
            ISalesRepository salesRepo,
            IPromotionsRepository promoRepo,
            IInventoryRepo inventoryRepo
        )
        {
            _salesRepo = salesRepo;
            _promoRepo = promoRepo;
            _inventoryRepo = inventoryRepo;
        }

        public Store GetStore()
        {
           return _salesRepo.GetStore();
        }

        public SaleInvoice StartNewSaleTransaction()
        {
           var sale = _salesRepo.CreateNewSaleInvoice();
           sale.InvoiceDate = DateTime.Now;
           return sale;
        }

        public bool CancelSale(int saleId)
        {
            var sale = _salesRepo.GetSale(saleId);
            if (sale == null)
            {
                return false;
            }
            else
            {               
                //   _salesRepo.DeleteSaleInvoice(saleId);
                if (sale.Finalised == true)
                {
                   var saleItems = _salesRepo.GetProductSalesInTransaction(saleId);
                    foreach (ProductSale item in saleItems)
                    {
                        _inventoryRepo.IncreaseProductQty(item.Id, 1);
                    }
                }
                _salesRepo.ClearProductSales(saleId);
                _salesRepo.DeleteSalePayments(saleId);
                return true;
            }
        }

        public void AddProductToSale(SaleInvoice sale, Product product)
        {
            ProductSale productSale = CreateProductSale(sale.Id, product);
            _salesRepo.AddProductSale(productSale);        
        }

        public SaleInvoice GetSale(int saleId)
        {
            return _salesRepo.GetSale(saleId);
        }

        public void ProcessProductSales(SaleInvoice sale, IList<ProductSale> productSales)
        {
            _salesRepo.AddProductSales(productSales);
            sale.Total = ProcessSaleTotalAmount(productSales);
            _salesRepo.UpdateSale(sale);            
        }

        public void DeleteSale(int saleId)
        {
            var sale = _salesRepo.GetSale(saleId);
            if (sale != null)
            {
                _salesRepo.ClearProductSales(saleId);
                _salesRepo.DeleteSalePayments(saleId);
                _salesRepo.DeleteSaleInvoice(saleId);
            }
        }

        // looks like a lot of for loops but must of them will only iterate 1-3 times      
        // note: clean up smelly code
        public IList<ProductSale> ApplyPromotionsToSale(int saleId, List<Product> products)
        {
            Dictionary<int, IList<Promotion>> productPromos = _promoRepo.GetProductActivePromotions();
            List<ProductSale> productSales = new List<ProductSale>();

            for (int p = 0; p < products.Count; p++)//for all products in a sale
            {
                if (products[p] == null) continue;
                int product = products[p].Id;
                if (productPromos.ContainsKey(product))//if any active promotions include the product
                {
                    List<Promotion> promos = (List<Promotion>) productPromos[product]; // all promos which include the product
                    PromotionComparer pc = new PromotionComparer();
                    promos.Sort(pc); //sorting promos by lowest cost Per product
                    bool promotionApplied = false; // cheapest promotion offer applicable will be selected 

                    foreach (var promo in promos)
                    {
                        int qtyNeeded = promo.Quantity; //Counter to check if sale includes the min qty required for promotion offer
                        List<Product> productList = new List<Product>(products); // new list as, a productId will be removed once counted
                        HashSet<int> promoProductList = new HashSet<int>(promo.ProductPromotions.Select(p => p.ProductId));
                        List<ProductSale> pontentialPromos = new List<ProductSale>();

                        for (int j = p; j < productList.Count; j++)
                        {
                            if (productList[j] != null && promoProductList.Contains(productList[j].Id))
                            {
                                qtyNeeded--;
                                int unitPrice = promo.PromotionPrice / promo.Quantity;
                                int remainder = (unitPrice % promo.PromotionPrice);
                                int firstUnitPrice = unitPrice + remainder;
                                if(j == p)
                                    pontentialPromos.Add(CreateProductSale(saleId, productList[j], promotion: promo, sellingPrice: firstUnitPrice));
                                else
                                    pontentialPromos.Add(CreateProductSale(saleId, productList[j], promotion: promo, sellingPrice: unitPrice));
                                productList[j] = null;
                            }
                            if (qtyNeeded == 0)
                            {
                                promotionApplied = true;
                                productSales.AddRange(pontentialPromos);
                                //All products included in this promotion will no longer be included in the productId list 
                                products = productList;
                                break;
                            }
                        }
                        if (promotionApplied) break;
                    }
                    if (!promotionApplied) productSales.Add(CreateProductSale(saleId, products[p]));
                }
                else
                {
                    productSales.Add(CreateProductSale(saleId, products[p]));
                }
            }
            return productSales;
        }

        public int ProcessSaleTotalAmount( ICollection<ProductSale> productSales )
        {        
            return productSales.Sum(p => p.PriceSold);
        }

        public ICollection<ProductSale> GetTransactionsProductSales(int saleId)
        {
           return _salesRepo.GetProductSalesInTransaction(saleId);
        }

        //helper func which returns a productSale obj
        public ProductSale CreateProductSale(int saleId, Product product, Promotion promotion = null, int  sellingPrice = 0)
        {
            ProductSale productSale = new ProductSale
            {
                SaleInvoiceId = saleId,
                ProductId = product.Id,
                Product = product,
                PriceSold = product.Price,
                PromotionApplied = false     
            };
            if (promotion != null)
            {
                productSale.PriceSold = sellingPrice;
                productSale.PromotionApplied = true;
                productSale.PromotionId = promotion.Id;
                productSale.Promotion = promotion;
            }
            return productSale;
        }

        public bool ProcessPayments( ICollection<Payment> payments )
        {
            var saleId = payments.First().SaleInvoiceId;
            var sale = _salesRepo.GetSale(saleId);
            if (sale == null || sale.Finalised == true)
            {
                return false;
            }
            else if (ValidatePaymentAmount(sale.Total, payments))
            {
                _salesRepo.AddSalesPayments(payments);
                FinaliseSale(sale);
                return true;
            }
            return false;
        }

        public bool ValidatePaymentAmount(int totalOwing, ICollection<Payment> payments)
        {
            int paymentTotal = payments.Sum(payment => payment.Amount);
            return (paymentTotal == totalOwing);
        }

        public void FinaliseSale(SaleInvoice sale)
        {
            sale.Finalised = true;            
            _salesRepo.UpdateSale(sale);
            foreach(var productSale in sale.ProductSales)
            {
                _inventoryRepo.DecreaseProductQty(productSale.ProductId, 1);
            }
        }

        //delete

        public int CalculateTotal(IList<ProductSale> productSales)
        {
            throw new NotImplementedException();
        }

        public Payment ProcessPayement(Payment payment, SaleInvoice sale)
        {
            throw new NotImplementedException();
        }

        public bool ValidateSalePayments(SaleInvoice sale)
        {
            throw new NotImplementedException();
        }




        //public List<ProductSale> CheckPromotionElgibility(List<Product> products , Promotion promo)
        //{

        //}

    }
}
