using InventoryPOS.api.Tests;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPOS.api.IntegrationTests;

public abstract class BaseApiControllerIntegrationTest : IClassFixture<TestWebApiFactory>
{
    protected readonly HttpClient _client;

    protected abstract string ResourceUnderTest { get; }

    public BaseApiControllerIntegrationTest(TestWebApiFactory factory)
    {    
        _client = factory.CreateClient();
    }
}
