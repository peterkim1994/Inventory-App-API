using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Repositories;
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
        private InventoryRepo _inventory { get; set; }

        public InventoryController(ILogger<InventoryController> logger, InventoryRepo inventoryRepo )
        {
            _logger = logger;
            _inventory = inventoryRepo;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost( Name="AddColour")]
        public IActionResult AddColour(Colour colour)
        {
            _inventory.AddColour(colour);
            _inventory.SaveChanges();
            return Ok(colour.ColourName);
        }
    }
}
