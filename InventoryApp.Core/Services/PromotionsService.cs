using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services.Interfaces;
using InventoryPOSApp.Core.Utils;

namespace InventoryPOSApp.Core.Services
{
    public class PromotionsService : IPromotionsService
    {
        private readonly IPromotionsRepository _repo;

        public PromotionsService(IPromotionsRepository promoRepo)
        {          
            _repo = promoRepo;
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
            var promo = _repo.GetPromotion(promotionId);
            if (promo != null)
            {
                _repo.ClearPromotionProducts(promotionId);
                promo.Active = false;
                _repo.EditPromotion(promo);
            }

        }
        public ICollection<ProductPromotion> GetProductPromotions()
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetPromotionsProducts(int promotionId)
        {
            return _repo.GetPromotionsProducts(promotionId);
        }

        public Promotion GetPromotion(int promotionId)
        {
            return _repo.GetPromotion(promotionId);
        }
    }
}
