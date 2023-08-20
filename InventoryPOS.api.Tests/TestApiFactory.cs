using InventoryPOS.DataStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace InventoryPOS.api.Tests
{
    internal class TestApiFactory : WebApplicationFactory<Program>
    {
        //https://www.youtube.com/watch?v=tj5ZCtvgXKY&ab_channel=MilanJovanovi%C4%87
        //todo: set up docker compose
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            //use DI for tests sd
            builder.ConfigureTestServices(services =>
            {
                services.Remove(services.Single(service => typeof(DbContextOptions<DBContext>) == service.ServiceType));
                var dbConnectionService = services.SingleOrDefault(srv => typeof(DbConnection) == srv.ServiceType); 

                if(dbConnectionService != null)
                {
                    services.Remove(dbConnectionService);
                }

                services.AddDbContext<DBContext>((_, option) => option.UseSqlServer("Server = localhost,5556;Database=pos_dev_db;Integrated Security=True"));              
            });
        }
    }
}
