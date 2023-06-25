using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Daos;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOSApp.Core.Repositories
{
    public class PromotionRepository : IPromotionsRepository
    {
        private DBContext _context { get; }

        public PromotionRepository(DBContext context)
        {
            _context = context;
        }



        public void AddProductPromotion(ProductPromotion productPromotion)
        {
            _context.ProductPromotions.Add(productPromotion);
            _context.SaveChanges();
        }


        public void AddPromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            _context.SaveChanges();
        }


        public void EditPromotion(Promotion promotion)
        {
            _context.Entry<Promotion>(promotion).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Promotion GetPromotionByName(string promotionName)
        {
            var product = _context.Promotions.FirstOrDefault(pr => pr.PromotionName == promotionName);
            return product;
        }

        public void ClearPromotionProducts(int promotionId)
        {
            var productPromos = from pp in _context.ProductPromotions
                                where pp.PromotionId == promotionId
                                select pp;
            _context.ProductPromotions.RemoveRange(productPromos);
            _context.SaveChanges();
        }

        public ProductPromotion GetProductPromotion(int productId, int promotionId)
        {
            return _context.ProductPromotions.FirstOrDefault
                   (
                       pp => pp.ProductId == productId && pp.PromotionId == promotionId
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
                            pr.Active == true &&
                            pr.Start <= DateTime.Now &&
                            pr.End >= DateTime.Now
                         select pr;
            return promos.ToList();
        }

        /// <summary>
        /// Returns a hash map/dictionary, keys are product Ids currently included in a promotion.
        /// values are a list of promotions which include that product
        /// </summary>
        /// <returns>A dictionary of products, and all promotions which include the product</returns>
        public Dictionary<int, IList<Promotion>> GetProductActivePromotions()
        {
            var promos = GetActivePromotions();
            Dictionary<int, IList<Promotion>> productPromos = new Dictionary<int, IList<Promotion>>(300);
            foreach (var promo in promos)
            {
                foreach (var prod in promo.ProductPromotions)
                {
                    int prodId = prod.ProductId;
                    if (!productPromos.ContainsKey(prodId))
                    {
                        productPromos[prodId] = new List<Promotion>();
                        productPromos[prodId].Add(promo);
                    }
                    else
                    {
                        productPromos[prodId].Add(promo);
                    }
                }
            }
            return productPromos;
        }

        public IList<Promotion> GetInactivePromotions()
        {
            var promos = from pr in _context.Promotions.Include(promo => promo.ProductPromotions)
                         where
                            pr.Start >= DateTime.Now ||
                            pr.End <= DateTime.Now
                         select pr;
            return promos.ToList();
        }

        public void RemoveProductPromotion(ProductPromotion productPromotion)
        {
            _context.ProductPromotions.Remove(productPromotion);
            _context.SaveChanges();
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

        public IList<Promotion> GetPromotionsByDate(DateTime rangeStart, DateTime rangeEnd)
        {
            throw new NotImplementedException();
        }

        public Promotion GetPromotion(int promotionId)
        {
            return _context.Promotions.Include(p => p.ProductPromotions).FirstOrDefault(p => p.Id == promotionId);
        }
    }
}
