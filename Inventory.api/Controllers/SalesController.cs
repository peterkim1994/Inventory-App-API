using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.api.Controllers;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Dtos;
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
        private readonly IMapper _mapper;

        public SalesController
        (
            ILogger<SalesController> logger,
            ISalesService service,
            IMapper mapper            
        )
        {
            _logger = logger;
            _service = service;
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


    }
        
}
