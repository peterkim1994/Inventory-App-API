using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;
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

        public void AddProductPromotion(ProductPromotion productPromotion)
        {
            _context.ProductPromotions.Add(productPromotion);
            _context.SaveChanges();
        }

        public void AddProductsToTransaction(IList<Product> products, SaleInvoice invoice)
        {
            throw new NotImplementedException();
        }

        public void AddPromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            _context.SaveChanges();
        }

        public Promotion CheckPromotionEligibility(IList<Product> productsToBeSold)
        {
            throw new NotImplementedException();
        }

        public bool CompleteTransaction(SaleInvoice invoice)
        {
            throw new NotImplementedException();
        }

        public void EditPromotion(Promotion promotion)
        {
            throw new NotImplementedException();
        }

        public IList<Promotion> GetCurrentPromotions()
        {
            throw new NotImplementedException();
        }

        public IList<Promotion> GetPromotionsByDate(DateTime rangeStart, DateTime rangeEnd)
        {
            throw new NotImplementedException();
        }

        public bool IsInvoiceFinalised(SaleInvoice invoice)
        {
            throw new NotImplementedException();
        }

        public Promotion GetPromotionByName(string promotionName)
        {
            var product = _context.Promotions.FirstOrDefault(pr => pr.PromotionName == promotionName);
            return product;
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public ProductPromotion GetProductPromotion(int productId, int promotionId)
        {
            return _context.ProductPromotions.FirstOrDefault
                   (
                       pp => pp.ProductId== productId && pp.PromotionId == promotionId
                   );
        }

        public IList<Promotion> GetPromotionsByDateRange(DateTime start, DateTime end)
        {
            var promos = from pr in _context.Promotions.Include(promo => promo.ProductPromotions)
                         where                        
                            pr.Start <= start &&
                            pr.End >= end
                         select pr;
            return promos.ToList();
        }


        public IList<Promotion> GetActivePromotions()
        {
            var promos = from pr in _context.Promotions.Include(promo => promo.ProductPromotions)
                         where
                            pr.Start <= DateTime.Now &&
                            pr.End >= DateTime.Now                         
                         select pr;
            return promos.ToList();
        }

        public IList<Promotion> GetInactivePromotions()
        {
            throw new NotImplementedException();
        }

        public void RemoveProductPromotion(ProductPromotion productPromotion)
        {
            _context.ProductPromotions.Remove(productPromotion);
            SaveChanges();
        }

        public IList<Product> GetPromotionsProducts(int promotionId)
        {
           Promotion promo = _context.Promotions.Find(promotionId);
           if (promo == null)
                return null;
            var products = from
                             pp in _context.ProductPromotions//.Include(pp => pp.Product)                          
                           where
                              pp.PromotionId == promotionId
                           join prod in _context.Products
                                             .Include(pr => pr.Brand)
                                             .Include(pr => pr.Colour)
                                             .Include(pr => pr.Size)
                                             .Include(pr => pr.ItemCategory)
                           on pp.ProductId equals prod.Id 
                           select prod;

            return products.ToList();
        }

        public Promotion GetPromotion(int promotionId)
        {
            return _context.Promotions.Include(p=> p.ProductPromotions).FirstOrDefault(p => p.Id == promotionId);
        }
    }
}
