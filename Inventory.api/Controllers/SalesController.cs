using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.api.Controllers;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using InventoryPOSApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesService _saleService;
        private readonly IPromotionsService _promoService;
        private readonly ISalesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IInventoryService _inventoryService;

        public SalesController
        (
            ILogger<SalesController> logger,
            ISalesRepository repo,
            ISalesService salesService,
            IPromotionsService promoService,
            IInventoryService inventoryService,
            IMapper mapper
        )
        {
            _logger = logger;
            _saleService = salesService;
            _promoService = promoService;
            _repo = repo;
            _mapper = mapper;
            _inventoryService = inventoryService;
        }

        [HttpGet("GetStore")]
        public Store GetStore()
        {
            return _saleService.GetStore();
        }

        [HttpPost("AddPromotion")]
        public IActionResult AddPromotion(PromotionDto promotionDto)
        {
            if (ModelState.IsValid)
            {
                var promotion = _mapper.Map<PromotionDto, Promotion>(promotionDto);
                if (promotion.Start > promotion.End)
                {
                    return BadRequest("The promotions start date must be prior to the end date");
                }
                if (_promoService.AddPromotion(promotion))
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
            if (_promoService.AddProductToPromotion(productId, promotionId))
            {
                return Ok(new ProductPromotion { ProductId = productId, PromotionId = promotionId });
            }
            return BadRequest("This promotion already contains this product");
        }


        [HttpGet("GetActivePromotions")]
        public IActionResult GetActivePromotions()
        {
            var promotions = _promoService.GetActivePromotions();
            var PromotionDtos = _mapper.Map<IList<Promotion>, IList<PromotionDto>>(promotions);
            return Ok(PromotionDtos);
        }


        [HttpGet("GetPromotionsProducts/{promotionId}")]
        public IActionResult GetPromotionsProducts(int promotionId)
        {
            var products = _promoService.GetPromotionsProducts(promotionId);
            if (products == null)
                return BadRequest();
            var ProductDtos = _mapper.Map<IList<Product>, IList<ProductDto>>(products);
            return Ok(ProductDtos);
        }

        [HttpPost("AddProductPromotions")]
        public IActionResult AddProductPromotions(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());

            int promotionId = reqBody.promotionId;
            IList<int> productIds = reqBody.productIds.ToObject<IList<int>>();

            Promotion promo = _promoService.GetPromotion(promotionId);
            if (promo == null)
            {
                return BadRequest("promotion doesnt exist");
            }
            foreach (var productId in productIds)
            {
                _promoService.AddProductToPromotion(productId, promotionId);
            }
            var promoDto = _mapper.Map<Promotion, PromotionDto>(promo);
            return Ok(promoDto);
        }

        [HttpDelete("DeletePromotion")]
        public IActionResult DeletePromotion(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());
            int promotionId = reqBody.promotionId;

            _promoService.DeletePromotion(promotionId);
            return Ok();
        }

        [HttpDelete("RemoveProductPromotions")]
        public IActionResult RemoveProductPromotions(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());

            int promotionId = reqBody.promotionId;
            IList<int> productIds = reqBody.productIds.ToObject<IList<int>>();

            Promotion promo = _promoService.GetPromotion(promotionId);
            if (promo == null)
            {
                return BadRequest("promotion doesnt exist");
            }
            foreach (var productId in productIds)
            {
                _promoService.RemoveProductPromotion(productId, promotionId);
            }
            var promoDto = _mapper.Map<Promotion, PromotionDto>(promo);
            return Ok(promoDto);
        }

        [HttpPut("EditPromotion")]
        public IActionResult EditPromotion(PromotionDto promotionDto)
        {
            Promotion promo = _mapper.Map<PromotionDto, Promotion>(promotionDto);
            if (ModelState.IsValid)
            {
                _promoService.EditPromotion(promo);
                var promoDto = _mapper.Map<Promotion, PromotionDto>(promo);
                return Ok(promoDto);
            }
            if (String.IsNullOrEmpty(promotionDto.PromotionName))
                return BadRequest("You must provide a name for the promotion");
            if (promotionDto.Quantity <= 0)
                return BadRequest("You must provide a min quantity for the promotion");
            if (promotionDto.PromotionPrice <= 0)
                return BadRequest("You must provide a price offer for the promotion");
            return BadRequest("Please provide valid promotion details");
        }

        [HttpPost("test")]
        public IActionResult Test(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());
            //     List<int> productIds = reqBody.productIds.ToObject<IList<int>>();
            List<Product> products = _inventoryService.GetProducts(reqBody.productIds.ToObject<IList<int>>());
            var promos = _saleService.ApplyPromotionsToSale(1, products);
            var dtos = _mapper.Map<IList<ProductSale>, IList<ProductSaleDto>>(promos);
            return Ok(dtos);
        }

        [HttpPost("StartNewSale")]
        public SaleInvoiceDto StartNewSale()
        {                       
            var sale = _saleService.StartNewSaleTransaction();
            return _mapper.Map<SaleInvoice, SaleInvoiceDto>(sale);
        }

        [HttpGet("GetSale/{saleId}")]
        public IActionResult GetSale(int saleId)
        {
            if(saleId == 0)
            {
                return BadRequest("0 is not a valid saleId number");
            }
            var sale = _saleService.GetSale(saleId);
            if(sale == null)
            {
                return BadRequest("The sale for the invoice number you provided is invalid");
            }
            return Ok(_mapper.Map<SaleInvoice, SaleInvoiceDto>(sale));
        }

        [HttpPost("CancelSale")]
        public IActionResult CancelSale(int saleId)
        {
            if (saleId == 0)
            {
                return BadRequest("0 is not a valid saleId number");
            }
            var sale = _saleService.GetSale(saleId);
            if (sale != null)
            {
                _saleService.CancelSale(saleId);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("AddProductSales")]
        public IActionResult AddProductSales(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());
            //var productDtos = _inventoryService.GetProducts(reqBody.productIds.ToObject<List<int>>());
            //var saleProducts = _mapper.Map<List<ProductDto>, List<Product>>(productDtos);
            var saleProducts = _inventoryService.GetProducts(reqBody.productIds.ToObject<List<int>>());
            int saleId = reqBody.saleId;
            SaleInvoice sale = _saleService.GetSale(saleId);

            if (sale == null)
                return BadRequest("This sale does not exist");
            else if (sale.Finalised == true) 
                return BadRequest("This Sale Has Already Been Finalised");            
            else if (sale != null && saleProducts.Count > 0)
            {
                var productSales = _saleService.ApplyPromotionsToSale(sale.Id, saleProducts);
                _saleService.ProcessProductSales(sale, productSales);
                SaleInvoiceDto invoice = _mapper.Map<SaleInvoice, SaleInvoiceDto>(sale);
                invoice.Products = _mapper.Map<IList<ProductSale>,IList<ProductSaleDto>>(productSales);
                return Ok(invoice);
            }
            else
            {
                return BadRequest("There are no products to add");
            }
        }

        [HttpPost("AddSalePayments")]
        public IActionResult AddSalePayments(ICollection<PaymentDto> paymentDtos)
        {
            //dynamic req = JObject.Parse(json.ToString());
            //ICollection<PaymentDto> paymentDtos = req.payments.ToObject<ICollection<PaymentDto>>();
            ICollection<Payment> payments = _mapper.Map<ICollection<PaymentDto>, ICollection<Payment>>(paymentDtos);

            if(payments == null || payments.Count() == 0)
                return BadRequest("error refresh the page");

            if (payments.Any(p=> p.Amount < 0.0m))
            {
                return BadRequest("You cant have negative payments. Please provide valid payments for the sale");
            }

            bool paymentSuccess = _saleService.ProcessPayments(payments);

            if (paymentDtos == null || paymentDtos.Count() < 1)
            { 
                return BadRequest("The payments for sale was not completeted,.Please provide valid payments for the sale");
            }
            else if (paymentSuccess)
            {
                int saleId = payments.First().SaleInvoiceId;
                var finalInvoice = _saleService.GetSale(saleId);
                var finalInvoiceDto = _mapper.Map<SaleInvoice, SaleInvoiceDto>(finalInvoice);
                return Ok(finalInvoiceDto);
            }
            else
            {
                return BadRequest("Please make sure the payment totals equals exactly the same as the sale amount total");
            }
        }

        [HttpPost("ProcessRefund")]
        public IActionResult ProcessRefund(RefundDto refundDto)
        {
            var sale = _saleService.GetSale(refundDto.SaleInvoiceId);
            var refund = _mapper.Map<RefundDto, Refund>(refundDto);
            if(sale == null)
            {
                return BadRequest("That invoice number does not exist");
            }
            else if(refund.Amount > sale.Total)
            {
                return BadRequest("The refund amount can not be more than the amount paid for the sale");
            }
            else if (refund.Amount <= 0)
            {
                return BadRequest("The refund amount can not less or equal to zero");
            }
            else if (string.IsNullOrWhiteSpace(refund.Reason))
            {
                return BadRequest("You must provide a reason for the refund");
            }
            else if(refund.Reason.Length > 99)
            {
                return BadRequest("The reason must be under 100 characters");
            }
            else
            {
                refund.RefundDate = DateTime.Now;
                _repo.AddRefund(refund);
                return Ok("refund successfully processed");
            }
        }
    }

}
