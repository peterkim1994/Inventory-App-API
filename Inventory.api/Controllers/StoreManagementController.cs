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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreManagementController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesService _saleService;
        private readonly IPromotionsService _promoService;
        private readonly ISalesRepository _repo;
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
            _repo = repo;
            _mapper = mapper;
            _inventoryService = inventoryService;
            _mangementService = managementService;
        }


        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions([FromQuery(Name = "from")] string from, [FromQuery(Name = "to")] string to)
        {
            //add some validation
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            var transactions = _repo.GetSales(fromDate, toDate);
            var transactionDtos = _mapper.Map<IList<SaleInvoice>, IList<SaleInvoiceDto>>(transactions);

            var productSales = transactionDtos.SelectMany(s => s.Products);
            for(var j = 0; j < transactionDtos.Count(); j++)
            {
                var sale = transactionDtos[j];
                for(var i = 0; i< sale.Products.Count(); i++)
                {
                    var product = sale.Products[i].Product;
                    sale.Products[i].Product = sale.DateTime + "  " + product;
                }
            }

            if (transactions != null)
                 return Ok(transactionDtos);

            return BadRequest("error getting transactions with those dates");
        }
    }
}
