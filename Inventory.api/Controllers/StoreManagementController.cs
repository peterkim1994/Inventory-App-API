using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using InventoryPOSApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreManagementController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;

        private readonly ISalesService _saleService;

        private readonly IPromotionsService _promoService;

        private readonly ISalesRepository _salesRepo;

        private readonly IMapper _mapper;

        private readonly IInventoryService _inventoryService;

        private readonly IStoreManagementService _mangementService;

        public StoreManagementController
        (
            ILogger<SalesController> logger,
            ISalesRepository repo,
            ISalesService salesService,
            IPromotionsService promoService,
            IInventoryService inventoryService,
            IMapper mapper,
            IStoreManagementService managementService
        )
        {
            _logger = logger;
            _saleService = salesService;
            _promoService = promoService;
            _salesRepo = repo;
            _mapper = mapper;
            _inventoryService = inventoryService;
            _mangementService = managementService;
        }


        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions([FromQuery(Name = "from")] string from, [FromQuery(Name = "to")] string to)
        {
            //TODO: add some validation
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            var transactions = _salesRepo.GetSales(fromDate, toDate);

            if (transactions == null)
            {
                return Ok();
            }

            var transactionDtos = _mapper.Map<IList<SaleInvoice>, IList<SaleInvoiceDto>>(transactions);

            return Ok(transactionDtos); 
        }

        [HttpGet("GetReport")]
        public IActionResult GetReport([FromQuery(Name = "from")] string from, [FromQuery(Name = "to")] string to)
        {
            //add some validation
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);

            SalesReportDto report = _mangementService.GetSalesReport(fromDate, toDate);

            if(report == null)
            {
                return BadRequest("system error report couldnt be generated");
            }

            return Ok(report);
        }

        [HttpPost("VoidProductSale")]
        [Authorize(Roles = "shopAdmin")]
        public IActionResult DeleteProductSale(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());

            int saleId = reqBody.saleId;
            int productSaleId = reqBody.productSaleId;

            bool productSaleDeleted = _mangementService.VoidSale(saleId, productSaleId);

            if (productSaleDeleted)
            {
                var sale = _mapper.Map<SaleInvoice, SaleInvoiceDto>(_saleService.GetSale(saleId));
                return Ok(sale);
            }

            return BadRequest("Product sale couldnt not be deleted");            
        }

        [HttpPost("RestockProductSales")]
        public IActionResult RestockProductSales(ICollection<int> productSaleIds)
        {
            if (productSaleIds == null)
            {
                return BadRequest();
            }

            var productSales = _salesRepo.GetProductSales(productSaleIds);
            foreach (var ps in productSales)
            {
                ps.Restocked = true; 
            }

            _salesRepo.UpdateProductSales(productSales);

            return Ok();
        }
    }
}
