using InventoryPOS.DataStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Testcontainers.MsSql;
using DotNet.Testcontainers.Builders;

namespace InventoryPOS.api.Tests;

public class TestWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    //https://www.youtube.com/watch?v=tj5ZCtvgXKY&ab_channel=MilanJovanovi%C4%87
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                                                         .WithImage("pkim333/test_database:v1.5")                                                            
                                                         .WithPortBinding(25565, 1433)                                                                                      
                                                         .WithHostname("localhost")
                                                         .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("RESTORE DATABASE successfully processed"))//.UntilFileExists("/var/opt/mssql/data/pos_test_db.mdf"))
                                                         .Build();

    private string _testDbConnectionString => $"Server = {_msSqlContainer.Hostname},25565;Database=pos_test_db;User Id=sa;Password=SomeStrongPassword1!";

    public async Task InitializeAsync()
    {       
        await _msSqlContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _msSqlContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.Single(service => typeof(DbContextOptions<DBContext>) == service.ServiceType));
            var dbConnectionService = services.SingleOrDefault(srv => typeof(DbConnection) == srv.ServiceType);

            if(dbConnectionService != null)
            {
                services.Remove(dbConnectionService);
            }

            services.AddDbContext<DBContext>((_, option) => option.UseSqlServer(_testDbConnectionString));              
        });
    }
}
