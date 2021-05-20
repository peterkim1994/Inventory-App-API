using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Comparers;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Utils;

namespace InventoryPOSApp.Core.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _salesRepo;
        private readonly IPromotionsRepository _promoRepo;

        public SalesService(ISalesRepository salesRepo, IPromotionsRepository promoRepo)
        {
            _salesRepo = salesRepo;
            _promoRepo = promoRepo;
        }

        public SaleInvoice StartNewSaleTransaction()
        {
            return _salesRepo.CreateNewSaleInvoice();
        }

        public bool CancelSale(int saleId)
        {
            var sale = _salesRepo.GetSale(saleId);
            if (sale != null || sale.Finalised == false)
            {
                _salesRepo.ClearProductSales(saleId);
                _salesRepo.DeleteSalePayments(saleId);
                _salesRepo.DeleteSaleInvoice(saleId);
                return true;
            }
            return false;
        }


        // looks like a lot of for loops but must of them will only iterate 1-3 times at most
        //Optimise later, use hashsets
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
                    List<Promotion> promos = (List<Promotion>)productPromos[product]; // all promos which include the product
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
                                pontentialPromos.Add(ProcessProductSale(saleId, productList[j], promotion: promo));
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
                    if (!promotionApplied) productSales.Add(ProcessProductSale(saleId, products[p]));
                }
                else
                {
                    productSales.Add(ProcessProductSale(saleId, products[p]));
                }
            }
            return productSales;
        }

        public int CalculateTotal(int saleInvoiceId)
        {
            ICollection<Product> products = _salesRepo.GetProductsInTransaction(saleInvoiceId);
            throw new NotImplementedException();
        }

        public ProductSale ProcessProductSale(int saleId, Product product, Promotion promotion = null)
        {
            ProductSale productSale = new ProductSale
            {
                SalesInvoiceId = saleId,
                ProductId = product.Id,
                Product = product,
                PriceSold = product.Price,
                PromotionApplied = false,
                PromotionId = 0,
            };
            if (promotion != null)
            {
                productSale.PriceSold = promotion.PromotionPrice / promotion.Quantity;
                productSale.PromotionApplied = true;
                productSale.PromotionId = promotion.Id;
                productSale.Promotion = promotion;
            }
            return productSale;
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
