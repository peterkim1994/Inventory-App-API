using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Inventory.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<InventoryController> _logger;
        private IInventoryRepo _inventory { get; set; }
        private IInventoryService _service { get; set; }
        private readonly IMapper _mapper;

        public InventoryController
        (
            ILogger<InventoryController> logger,
            IInventoryRepo inventoryRepo,
            IInventoryService service,
            IMapper mapper
        )
        {
            _logger = logger;
            _inventory = inventoryRepo;
            _service = service;
            _mapper = mapper;
        }


        [HttpGet("ProductAttributes")]
        public IActionResult GetAttributes()
        {
            return Ok(_service.GetProductAttributes());
        }

        [HttpGet]
        public IActionResult GetProducts()
        {            
            var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(_service.GetAllProducts());
            return Ok(productDtos);
        }

        [HttpPost("AddColour", Name ="AddColour")]
        [Route("[controller]/[action]")]
        public IActionResult AddColour(Colour colour)
        {
            if (_service.AddColour(colour))
            {
                var colourDto = _mapper.Map<Colour, ColourDto>(colour);
                return Ok(colourDto);
            }
            return BadRequest("Colour Already Exists");               
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            if (_service.AddProduct(product))
            {
                var productDto = _mapper.Map<Product, ProductDto>(product);
                return Ok(productDto);
            }
            return BadRequest("Product already exists");
        }

        [HttpPost("AddSize")]
        public IActionResult AddSize(Size size)
        {
            if (_service.AddSize(size))
            {
                return Ok(size);
            }
            return BadRequest("Size already exists");
        }


        [HttpPost("AddCategory")]
        public IActionResult AddItemCategory(ItemCategory category)
        {
            if (_service.AddItemCategory(category))
            {
                return Ok(category);
            }
            return BadRequest("Category Already Exists");
        }

        [HttpPost("AddBrand")]
        public IActionResult AddBrand(Brand brand)
        {
            if (_service.AddBrand(brand))
            {
                return Ok(brand);
            }
            return BadRequest("Brand already exists");
        }

    
    }
}
