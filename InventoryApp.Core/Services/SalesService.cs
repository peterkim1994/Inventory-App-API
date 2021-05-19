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
            if (_repo.GetProductPromotion(productId, promotionId) == null)
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
            return false;
        }

        // looks like a lot of for loops but must of them will only iterate 1-3 times at most
        //Optimise later, use hashsets
        public IList<ProductSale> ApplyPromotions(int saleId, List<Product> products)
        {          
            Dictionary<int, IList<Promotion>> productPromos = _repo.GetProductActivePromotions();
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
                            if (productList[j] !=null && promoProductList.Contains(productList[j].Id))
                            {
                                qtyNeeded--;
                                pontentialPromos.Add(ProcessProductSale(saleId, productList[j], promotion : promo ));
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
                    if(!promotionApplied) productSales.Add(ProcessProductSale(saleId, products[p]));
                }
                else
                {
                    productSales.Add(ProcessProductSale(saleId, products[p]));
                }
            }
            return productSales;
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
            if(promotion != null)
            {
                productSale.PriceSold = promotion.PromotionPrice / promotion.Quantity;
                productSale.PromotionApplied = true;
                productSale.PromotionId = promotion.Id;
                productSale.Promotion = promotion;
            }
            return productSale;
        }

       
        //public List<ProductSale> CheckPromotionElgibility(List<Product> products , Promotion promo)
        //{

        //}

    }
}
