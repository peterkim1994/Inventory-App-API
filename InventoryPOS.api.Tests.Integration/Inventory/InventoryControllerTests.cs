using InventoryPOS.api.IntegrationTests;
using InventoryPOSApp.Core.Models.ResponseModels;
using Newtonsoft.Json;
using System.Net;

namespace InventoryPOS.api.Tests.Integration.Inventory;

public class InventoryControllerTests : BaseApiControllerIntegrationTest
{
    protected override string ResourceUnderTest => "inventory";

    public InventoryControllerTests(TestWebApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [MemberData(nameof(GetTestCatalogRequests))]
    [Trait("Inventory", "GetInventoryProducts")]
    public async Task GetProduct_WithValidQueryParams_IsSuccessfullResult(int numItemsPerPage, int startPage, int pageLoadBuffer, int expectedNumProductsReturned)
    {
        HttpResponseMessage response = await _client.GetAsync($"{ResourceUnderTest}/GetInventoryProducts/?storeId=1&numItemsPerPage={numItemsPerPage}&startPage={startPage}&pageLoadBuffer={pageLoadBuffer}");
    
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            InventoryCatalogModel? catelogResponseDto = JsonConvert.DeserializeObject<InventoryCatalogModel>(jsonResponse)!;
            bool anyProductsNotFullyLoaded = catelogResponseDto.IncludedProducts.Any(p => p.ColourValue == null || p.BrandValue == null || p.SizeValue == null || p.ItemCategoryValue == null);
            
            Assert.NotNull(catelogResponseDto);
            Assert.Equal(expectedNumProductsReturned, catelogResponseDto.IncludedProducts.Count());
            Assert.False(anyProductsNotFullyLoaded);
            //add assert for total available pages
        }

        Assert.True(response.IsSuccessStatusCode);
    }

    public static IEnumerable<Object[]> GetTestCatalogRequests()
    {
        yield return new Object[] { 100, 1, 3, 300};
        yield return new Object[] { 500, 1, 3, 1500 };
        yield return new Object[] { 10, 1, 3, 30 };
    }
}
