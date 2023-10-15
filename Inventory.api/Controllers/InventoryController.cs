using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Models.QueryModels;
using InventoryPOSApp.Core.Models.ResponseModels;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using InventoryPOSApp.Core.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Inventory.api.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;

    private IInventoryRepo _inventoryRepo { get; set; }

    private IInventoryService _inventoryService { get; set; }

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
        _inventoryRepo = inventoryRepo;
        _inventoryService = service;
        _mapper = mapper;
    }

    [HttpGet("ProductAttributes")]
    public IActionResult GetAttributes()
    {
        return Ok(_inventoryService.GetProductAttributes());
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        List<ProductDto> productDtos = _mapper.Map<List<Product>, List<ProductDto>>(_inventoryService.GetAllProducts());
        return Ok(productDtos);
    }

    [HttpGet("GetInventoryProducts")]
    //[Authorize(Roles = "shopAdmin, staff")]
    public async Task<IActionResult> GetInventoryCatelog([FromQuery] InventoryCatelogRequest inventoryCatalogRequest)
    {
        //todo: get store id from token claim
        inventoryCatalogRequest.StoreId = 1;

        (IList<Product> products, int totalItems) = await _inventoryService.GetInventoryCatelogAsync(inventoryCatalogRequest);

        if(totalItems == 0)
        {
            return BadRequest("You have no products in your inventory");
        }

        var inventoryCatalog = new InventoryCatalogModel
        {
            IncludedProducts = _mapper.Map<IList<Product>, List<ProductDto>>(products),
            StoreId = inventoryCatalogRequest.StoreId,
            AvailableNumberOfPages = (int) Math.Ceiling( totalItems/ (double) inventoryCatalogRequest.NumItemsPerPage)
        };

        return Ok(inventoryCatalog);
    }

    [HttpGet("GetInventorySearch")]
    public async Task<IActionResult> SearchInventory([FromQuery] InventorySearchQuery searchQuery)
    {
        //todo: get store id from token claim
        searchQuery.StoreId = 1;

        var products = await _inventoryRepo.SearchProductsAsync(searchQuery);
        var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(products);

        return Ok(productDtos);
    }

    [HttpGet("GetTheseProducts")]
    public IActionResult GetProducts([FromQuery] ICollection<int> productIds)
    {
        if (productIds != null && productIds.Count > 0)
        {
            var products = _inventoryService.GetProductsWithAttributes(productIds);
            var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(products)
                                .Where(p => productIds.Contains(p.Id)).ToList();
            return Ok(productDtos);
        }

        return BadRequest();
    }

    [HttpPost("AddColour", Name = "AddColour")]
    [Route("[controller]/[action]")]
    public IActionResult AddColour(Colour colour)
    {
        if (_inventoryService.AddColour(colour))
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
            return BadRequest("invalid product");
        }

        var product = _mapper.Map<ProductDto, Product>(productDto);

        if (_inventoryRepo.ContainsProduct(product))
        {
            //hack -- user does insists products can have the same description & attributes and still be differientiated by price     
            product.Description += "Price: " + product.Price;

            if (_inventoryRepo.ContainsProduct(product))
            {
                throw new InvalidOperationException("Product already exists");
            }

            if (_inventoryService.AddProduct(product))
            {
                productDto = _mapper.Map<Product, ProductDto>(product);

                return Ok(productDto);
            }
        }

        if (_inventoryService.AddProduct(product))
        {
            productDto = _mapper.Map<Product, ProductDto>(product);
            //return CreatedAtRoute("AddProduct", new { productDto.Id }, productDto);

            return Ok(productDto);
        }
        else if (product.Barcode != 0 && _inventoryService.BarcodeIsAvailable(product.Barcode))
        {
            return BadRequest("Barcode already exists");
        }

        return BadRequest("Product already exists");
    }

    [HttpPost("EditProduct")]
    [Authorize(Roles = "shopAdmin")]
    public IActionResult EditProduct(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            Product productToUpdate = _inventoryRepo.GetProduct(productDto.Id);

            if (productDto.Barcode != productToUpdate.Barcode)
            {
                return BadRequest("A product's barcode can not be changed");
            }

            var product = _mapper.Map<ProductDto, Product>(productDto);
            _inventoryService.EditProduct(product);

            productToUpdate = _inventoryRepo.GetProduct(product.Id);
            productDto = _mapper.Map<Product, ProductDto>(productToUpdate);

            return Ok(productDto);
        }

        return BadRequest("Product Details are not valid");
    }


    [HttpPost("AddSize")]
    [Authorize(Roles = "shopAdmin")]
    public IActionResult AddSize(Size size)
    {
        if (_inventoryService.AddSize(size))
        {
            return Ok(size);
        }

        return BadRequest("Size already exists");
    }

    [HttpPost("AddCategory")]
    public IActionResult AddItemCategory(ItemCategory category)
    {
        if (_inventoryService.AddItemCategory(category))
        {
            return Ok(category);
        }

        return BadRequest("Category Already Exists");
    }


    [HttpPost("AddBrand")]
    public IActionResult AddBrand(Brand brand)
    {
        if (_inventoryService.AddBrand(brand))
        {
            return Ok(brand);
        }

        return BadRequest("Brand already exists");
    }

    [HttpPost("EditBrand")]
    [Authorize(Roles = "shopAdmin")]
    public IActionResult EditBrand(Brand brand)
    {
        if (ModelState.IsValid)
        {
            if (_inventoryService.EditBrand(brand))
            {
                return Ok(brand);
            }

            return BadRequest("This brand already exists");
        }
        else
        {
            return BadRequest("Improper brand name format");
        }
    }

    [HttpPost("EditCategory")]
    [Authorize(Roles = "shopAdmin")]
    public IActionResult EditItemCategory(ItemCategory category)
    {
        if (ModelState.IsValid)
        {
            if (_inventoryService.EditCategory(category))
            {
                return Ok(category);
            }

            return BadRequest("This category already exists");
        }
        else
        {
            return BadRequest("Improper item category format");
        }
    }

    [Authorize(Roles = "shopAdmin")]
    [HttpPost("EditSize")]
    public IActionResult EditSize(Size size)
    {
        if (ModelState.IsValid)
        {
            if (_inventoryService.EditSize(size))
            {
                return Ok(size);
            }
            else
            {
                return BadRequest("This size already exists");
            }
        }

        return BadRequest("Improper size name format");
    }

    private ProductAttribute GetAttribute(List<IEnumerable<InventoryPOS.DataStore.Daos.Interfaces.ProductAttribute>> atts, string val)
    {
        var attss = atts.SelectMany(a => a);
        return attss.FirstOrDefault(a => a.Value.Equals(val));
    }
}