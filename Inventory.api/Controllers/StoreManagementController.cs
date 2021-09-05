using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            var transactions = _mangementService.GetPreviousSales(fromDate, toDate);
            return Ok(transactions);

        }
    }
}
