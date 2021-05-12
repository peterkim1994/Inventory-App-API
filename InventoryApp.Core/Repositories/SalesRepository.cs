using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InventoryPOS.DataStore;
using InventoryPOS.DataStore.Models;

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
             return _context.Promotions.FirstOrDefault(pr => pr.PromotionName == promotionName);     
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
    }
}
