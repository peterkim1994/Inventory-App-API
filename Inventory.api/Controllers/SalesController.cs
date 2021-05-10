using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.api.Controllers;
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
        private IInventoryService _service { get; set; }

        private readonly IMapper _mapper;
    }
        
}
