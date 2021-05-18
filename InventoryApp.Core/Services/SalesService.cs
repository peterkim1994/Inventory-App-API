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
        private readonly ISalesRepository _repo;

        public SalesService(ISalesRepository repo)
        {
            _repo = repo;
        }

        public bool AddPromotion(Promotion promotion)
        {
            promotion.PromotionName = TextProcessor.ToTitleCase(promotion.PromotionName);
            if (_repo.GetPromotionByName(promotion.PromotionName) == null)
            {
                _repo.AddPromotion(promotion);
                return true;
            }     
            return false;            
        }

        public bool AddProductToPromotion(int productId, int promotionId)
        {
            if(_repo.GetProductPromotion(productId, promotionId) == null)
            {
                ProductPromotion productPromotion = new ProductPromotion { ProductId = productId, PromotionId = promotionId };
                _repo.AddProductPromotion(productPromotion);
                return true;
            }
            return false;
        }

        public IList<Promotion> GetActivePromotions()
        {
            return _repo.GetActivePromotions();
        }


        public void EditPromotion(Promotion promotion)
        {
           promotion.PromotionName = TextProcessor.ToTitleCase(promotion.PromotionName);
            _repo.EditPromotion(promotion);
        }

        public bool RemoveProductPromotion(int productId, int promotionId)
        {
            var productPromo = _repo.GetProductPromotion(productId, promotionId); 
            if (productPromo == null)
            {
                return false;
            }
            _repo.RemoveProductPromotion(productPromo);
            return true;
        }

        public void DeletePromotion(int promotionId)
        {
            _repo.ClearPromotionProducts(promotionId);
            var promo = _repo.GetPromotion(promotionId);
            promo.Active = false;         
            _repo.EditPromotion(promo);
        }

        public int CalculateTotal(int saleInvoiceId)
        {
           ICollection<Product> products = _repo.GetProductsInTransaction(saleInvoiceId);
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

        public ICollection<ProductPromotion> GetProductPromotions()
        {
            throw new NotImplementedException();
        }

        public SaleInvoice StartSaleTransaction()
        {
            var sale = _repo.GetPreviousSale();
            if (sale.Finalised == true)
                return _repo.CreateNewSaleInvoice();
            else
            {
                _repo.ClearProductSales(sale.Id);
                return _repo.CreateNewSaleInvoice();
            }
        }

        public bool CancelPrevSale()
        {
            throw new NotImplementedException();
        }

        public bool AddProductToSale(int SaleId, int productId)
        {
           var sale = _repo.GetCurerntSale
        }

        // looks like a lot of for loops but must of them will only iterate 1-3 times at most
        //Optimise later, use hashsets
        public IList<Promotion> CheckEligibalePromotions(IList<int> productIds)
        {
            Dictionary<int, IList<Promotion>> productPromos = _repo.GetProductActivePromotions();
            IList<Promotion> eligiblePromotions = new List<Promotion>();

            //foreach(var product in productIds)//for all products in a sale
            for (int p = 0; p < productIds.Count; p++)
            {
                int product = productIds[p];
                if (productPromos.ContainsKey(product))//if any active promotions include the product
                {
                    List<Promotion> promos = (List<Promotion>) productPromos[product]; // all promos which include the product
                    PromotionComparer pc = new PromotionComparer();
                    promos.Sort(pc); //sorting promos by lowest cost Per product
                    Promotion bestPromotion = null; // cheapest promotion offer applicable will be selected 
                    foreach (var promo in promos)
                    {
                        int qtyNeeded = promo.Quantity; //Counter to check if sale includes the min qty required for promotion offer
                        List<int> productList = new List<int>(productIds); // new list as, a productId will be removed once counted
                        for (int j = p; j < productList.Count; j++)
                      //      foreach (var prod in productList)
                        {
                            int prod = productList[j];
                            for (int i = 0; i < promo.ProductPromotions.Count; i++)
                            {
                                int promoProduct = promo.ProductPromotions[i].ProductId;
                                if (productList.Contains(promoProduct))
                                {
                                    qtyNeeded--;
                                    productList[j] = 0;
                                }                                  
                                if (qtyNeeded == 0)
                                {
                                    bestPromotion = promo;
                                    //All products included in the promotion will no longer be included in the productId list,
                                    //as they are no longer applicable for other promos
                                    productIds = productList; 
                                    break;
                                }                                  
                            }
                          if(bestPromotion != null)  break;
                        }
                        if (bestPromotion != null) break;
                    }
                    if(bestPromotion != null) /// if null it means there the min qty for applicable products for promo wasnt reached
                        eligiblePromotions.Add(bestPromotion);
                }
            }
            return eligiblePromotions;
        }        

        public List<ProductSale> ProcessPromotions(List<Product> products, int saleId)
        {
            Dictionary<int, IList<Promotion>> productPromos = _repo.GetProductActivePromotions();
            List<ProductSale> productSales = new List<ProductSale>();
            for (int i = 0; i < products.Count; i++)
            {
                Product product = products[i];
                if (productPromos.ContainsKey(product.Id))
                {
                    List<Promotion> promosIncludingProduct = (List<Promotion>) productPromos[product.Id];
                    promosIncludingProduct.Sort(new PromotionComparer());
                    
                    foreach(var promo in promosIncludingProduct)
                    {
                        int qtyNeeded = promo.Quantity; //Counter to check if sale includes the min qty required for promotion offer
                        if (qtyNeeded == 1)
                        {
                            ProductSale productSale = new ProductSale
                            {
                                SalesInvoiceId = saleId,
                                ProductId = product.Id,
                                PriceSold = promo.PromotionPrice,
                                PromotionId = promo.Id,
                                PromotionApplied = true
                            };
                            productSales.Add(productSale);
                            break;
                        }
                        List<ProductSale> promoProducts = new List<ProductSale>(); // new list as, a productId will be removed once counted
                        for (int j = i; j < products.Count; j++)
                        {
                            if (promo.ProductPromotions.FirstOrDefault(pp => pp.ProductId == product.Id) != null)
                            {
                                qtyNeeded--;
                                promoProducts.Add(new ProductSale
                                {
                                    SalesInvoiceId = saleId,
                                    ProductId = product.Id,
                                    PriceSold = promo.PromotionPrice/promo.Quantity,
                                    PromotionId = promo.Id,
                                    PromotionApplied = true
                                });

                            }                             
                            if(qtyNeeded == 0)
                            {

                            }
                        }
                    }
                }
                else
                {
                    ProductSale productSale = new ProductSale
                    {
                        SalesInvoiceId = saleId,
                        ProductId = product.Id,
                        PriceSold = product.Price,
                        PromotionApplied = false
                    };
                    productSales.Add(productSale);
                }
            }
            return productSales;
        }

        //public List<ProductSale> CheckPromotionElgibility(List<Product> products , Promotion promo)
        //{

        //}

    }
}
