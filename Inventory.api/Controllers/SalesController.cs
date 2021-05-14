﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.api.Controllers;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;    
        private ISalesService _service { get; set; }
        private ISalesRepository _repo { get; set; }
        private readonly IMapper _mapper;

        public SalesController
        (
            ILogger<SalesController> logger,
            ISalesRepository repo,
            ISalesService service,
            IMapper mapper            
        )
        {
            _logger = logger;
            _service = service;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("AddPromotion")]
        public IActionResult AddPromotion(PromotionDto promotionDto)
        {
            if (ModelState.IsValid)
            {
                var promotion = _mapper.Map<PromotionDto, Promotion>(promotionDto);
                if (_service.AddPromotion(promotion))
                {
                    return Ok(promotionDto);
                }
                return BadRequest("that promtion already exists");
            }
            return BadRequest("Invalid promotion settings");         
        }

        [HttpPost("AddProductPromotion")]
        public IActionResult AddProductPromotion(int productId, int promotionId)
        {
            if(_service.AddProductToPromotion(productId, promotionId))
            {
                return Ok(new ProductPromotion { ProductId = productId, PromotionId = promotionId });
            }
            return BadRequest("This promotion already contains this product");
        }

        [HttpGet("GetActivePromotions")]
        public IActionResult GetActivePromotions()
        {
            var promotions = _service.GetActivePromotions();
            var PromotionDtos = _mapper.Map<IList<Promotion>, IList<PromotionDto>>(promotions);
            return Ok(PromotionDtos);
        }


        [HttpGet("GetPromotionsProducts/{promotionId}")]
        public IActionResult GetPromotionsProducts(int promotionId)
        {
            var products = _repo.GetPromotionsProducts(promotionId);
            if (products == null)
                return BadRequest();
            var ProductDtos = _mapper.Map<IList<Product>, IList<ProductDto>>(products);
            return Ok(ProductDtos);
        }

        [HttpPost("AddManyProductPromotions")]
        public IActionResult AddManyProductPromotions(int promoId, IList<int> productIds)
        {
            Promotion promo = _repo.GetPromotion(promoId);
            if(promo == null)
            {
                return BadRequest();
            }
            foreach (var productId in productIds)
            {
                _service.AddProductToPromotion(productId, promoId);
            }
            var promoDto = _mapper.Map<Promotion, PromotionDto>(promo);            
            return Ok(promoDto);
        }

        [HttpDelete("RemoveManyProductPromotions")]
        public IActionResult RemoveManyProductPromotions(int promoId, IList<int> productIds)
        {
            Promotion promo = _repo.GetPromotion(promoId);
            if (promo == null)
            {
                return BadRequest();
            }
            foreach (var productId in productIds)
            {
                _service.RemoveProductPromotion(productId, promoId);
            }
            var promoDto = _mapper.Map<Promotion, PromotionDto>(promo);
            return Ok(promoDto);
        }

    }
        
}
