﻿using System;
using System.Collections.Generic;
using System.Text;
using InventoryPOS.DataStore.Models;
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


        public Promotion EditPromotion(Promotion promotion)
        {
            throw new NotImplementedException();
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
    }
}