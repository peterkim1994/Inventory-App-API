using Xunit.Abstractions;

namespace InventoryPOS.api.Tests;

public class Sandbox
{

    private readonly ITestOutputHelper _output;

    public Sandbox(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test1()
    {
        var df = 23;
        Assert.Equal(23, df);
    }

    [Fact]
    public void Test2()
    {
        var df = 23;
        Assert.Equal(23, df);
    }

    [Theory]
    [InlineData("sd", "peter", "kim")]
    [InlineData("sd", "andrew", "kim")]
    [InlineData("sd", "kim", "kim")]
    public void TestRandomStuff(string e, string x, string y)
    {
        _output.WriteLine(e);
        _output.WriteLine(x);
        Assert.Equal(x, y);
        _output.WriteLine(y);
    }
}