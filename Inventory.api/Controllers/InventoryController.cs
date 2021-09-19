using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Daos;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Inventory.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
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
        [Authorize(Roles="shopAdmin")]
        public IActionResult GetAttributes()
        {
            return Ok(_service.GetProductAttributes());
        }

        [HttpGet]
        [Authorize(Roles="shopAdmin")]
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
        public IActionResult AddProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Product Details are invalid");
            }
            var product = _mapper.Map<ProductDto, Product>(productDto);

            if (_inventory.ContainsProduct(product))
            {
                return BadRequest("there is already a product with the same size, description, colour, category as this one");
            }
            if (_service.AddProduct(product))
            {               
                productDto = _mapper.Map<Product, ProductDto>(product);
             //   return CreatedAtRoute("AddProduct", new { productDto.Id }, productDto);
                return Ok(productDto);
            }
            else if (product.Barcode != 0 && _service.BarcodeIsAvailable(product.Barcode))
            {
                return BadRequest("That Barcode already exists");
            }
            return BadRequest("That Product manufacture code already belongs to an exsiting product");

        }

        [HttpPut("EditProduct")]
        public IActionResult EditProduct(ProductDto productDto)
        {
            //  var Product product = _mapper.Map<ProductDto, Product>(productDto);
            var product = _mapper.Map<ProductDto, Product>(productDto);
            if (ModelState.IsValid)
            {                
                _service.EditProduct(product);
                Product updatedProduct = _inventory.GetProduct(product.Id);
           //       var productDtos = _mapper.Map<Product, ProductDto>(updatedProduct);
                productDto = _mapper.Map<Product, ProductDto>(updatedProduct);
                return Ok(productDto);
            }
            return BadRequest("Product Details are not valid");
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

        [HttpPut("EditBrand")]
        public IActionResult EditBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {             
                if(_service.EditBrand(brand))
                  return Ok(brand);
                else
                  return BadRequest("This brand already exists");
            }
            else
            {
                return BadRequest("Improper brand name format");
            }
        }

        [HttpPut("EditCategory")]
        public IActionResult EditItemCategory(ItemCategory category)
        {
            if (ModelState.IsValid)
            {
                if (_service.EditCategory(category))
                    return Ok(category);
                else
                    return BadRequest("This category already exists");
            }
            else
            {
                return BadRequest("Improper item category format");
            }
        }

        [HttpPut("EditSize")]
        public IActionResult EditSize(Size size)
        {
            if (ModelState.IsValid)
            {
                if (_service.EditSize(size))
                    return Ok(size);
                else
                    return BadRequest("This size already exists");
            }
            else
            {
                return BadRequest("Improper size name format");
            }
        }
    }
}
