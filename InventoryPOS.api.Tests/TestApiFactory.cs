using InventoryPOS.DataStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Testcontainers;

namespace InventoryPOS.api.Tests;

internal class TestApiFactory : WebApplicationFactory<Program>
{
    //https://www.youtube.com/watch?v=tj5ZCtvgXKY&ab_channel=MilanJovanovi%C4%87
    //todo: set up docker compose
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                                                         .WithImage("pkim333/test_database")
                                                         .Build();

    private readonly string _testDbConnectionString = "Server = localhost,25565;Database=pos_test_db;User Id=sa;Password=SomeStrongPassword1!";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        //use DI for tests sd
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.Single(service => typeof(DbContextOptions<DBContext>) == service.ServiceType));
            var dbConnectionService = services.SingleOrDefault(srv => typeof(DbConnection) == srv.ServiceType);

            // service descripter for our DB context options for our database context
           // var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DBContext>));

            if(dbConnectionService != null)
            {
                services.Remove(dbConnectionService);
            }

            services.AddDbContext<DBContext>((_, option) => option.UseSqlServer(_testDbConnectionString));              
        });
    }
}
