using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Daos;
using InventoryPOS.DataStore.Daos.Interfaces;
using InventoryPOSApp.Core.Dtos;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using InventoryPOSApp.Core.Utils;
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
        [Authorize(Roles = "shopAdmin")]
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

        [HttpPost("GetTheseProducts")]
        public IActionResult GetProductsForPrinting(ICollection<int> productIds)
        {
            if (productIds != null || productIds.Count > 0)
            {
                var productDtos = _mapper.Map<List<Product>, List<ProductDto>>(_service.GetAllProducts())
                                    .Where(p => productIds.Contains(p.Id)).ToList();
                return Ok(productDtos);
            }

            return BadRequest();
        }

        [HttpPost("AddColour", Name = "AddColour")]
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
        [Authorize(Roles = "shopAdmin")]
        public IActionResult EditProduct(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                Product productToUpdate = _inventory.GetProduct(productDto.Id);
                if (productDto.Barcode != productToUpdate.Barcode)
                {
                    return BadRequest("A product's barcode can not be changed");
                }
                var product = _mapper.Map<ProductDto, Product>(productDto);
                _service.EditProduct(product);
                productToUpdate = _inventory.GetProduct(product.Id);
                productDto = _mapper.Map<Product, ProductDto>(productToUpdate);
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
        [Authorize(Roles = "shopAdmin")]
        public IActionResult AddBrand(Brand brand)
        {
            if (_service.AddBrand(brand))
            {
                return Ok(brand);
            }
            return BadRequest("Brand already exists");
        }

        [HttpPut("EditBrand")]
        [Authorize(Roles = "shopAdmin")]
        public IActionResult EditBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (_service.EditBrand(brand))
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
        [Authorize(Roles = "shopAdmin")]
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

        [Authorize(Roles = "shopAdmin")]
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

        [HttpPost("import")]
        public IActionResult ImportExcelSheet()
        {
            var lines = System.IO.File.ReadAllLines(@"C:\Users\peter\rand\inventory.csv").ToList();

            foreach (var item in lines.Skip(1))
            {
                var details = item.Split(",");
                List<IEnumerable<InventoryPOS.DataStore.Daos.Interfaces.ProductAttribute>> atts = _service.GetProductAttributes();

                var brandVal = TextProcessor.ToTitleCase(details[1]);
                Brand brand = GetAttribute(atts, brandVal) as Brand;              
                if (brand == null)
                {
                   var newBrand = new Brand { Value = brandVal };
                   _service.AddBrand(newBrand);
                    brand = newBrand;
                                      
                }

                var colourVal = TextProcessor.ToTitleCase(details[2]);
                Colour colour = (Colour)GetAttribute(atts, colourVal);
                if(colour == null)
                {
                    var newColour = new Colour() { Value = colourVal };
                    _service.AddColour(newColour);
                    colour = newColour;
                }

                var sizeVal = details[3].ToUpper();
                Size size = (Size)GetAttribute(atts, sizeVal);
                if(size == null)
                {
                    var newSize = new Size() { Value = sizeVal };
                    _service.AddSize(newSize);
                    size = newSize;
                }

                var categoryVal = TextProcessor.ToTitleCase(details[4]);
                ItemCategory category = (ItemCategory)GetAttribute(atts, categoryVal);

                if(category == null)
                {
                    var newCategory = new ItemCategory() { Value = categoryVal };
                    _service.AddItemCategory(newCategory);
                    category = newCategory;
                }

                var newProduct = new ProductDto()
                {
                    Active = true,
                    BrandId = brand.Id,
                    ColourId = colour.Id,
                    ItemCategoryId = category.Id,
                    SizeId = size.Id,
                    Description = details[5],
                    ManufactureCode = details[0],
                    Barcode = 0,
                    Qty = 50,
                    Price = Double.Parse(details[7])
                };
                AddProduct(newProduct);
            }

            return Ok();
        }

        private ProductAttribute GetAttribute(List<IEnumerable<InventoryPOS.DataStore.Daos.Interfaces.ProductAttribute>> atts, string val)
        {
           var attss = atts.SelectMany(a => a);
           return attss.FirstOrDefault(a => a.Value.Equals(val)); // atts.FirstOrDefault(att => attss.FirstOrDefault(a => a.Value.Equals(val));
        }
 
    }
}
