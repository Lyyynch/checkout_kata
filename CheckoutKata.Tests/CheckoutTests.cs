using CheckoutKata.Tests.Stubs;

namespace CheckoutKata.Tests;

public class CheckoutTests
{
    private readonly Checkout _checkout = new (new SkuRetrievalServiceStub(), new SkuSpecialRetrievalServiceStub());
    
    [Fact]
    public void NewCheckout_NoItemsScanned_ShouldReturnZero()
    {
        var total = _checkout.Total;
        Assert.Equal(0, total);
    }

    [Theory]
    [InlineData("A", 50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void NewCheckout_OneItemScanned_ShouldReturnTotal(string itemCode, int itemPrice)
    {
        _checkout.Scan(itemCode);
        var total = _checkout.Total;
        Assert.Equal(itemPrice, total);
    }

    [Theory]
    [InlineData("A", "B", 80)]
    [InlineData("B", "C", 50)]
    [InlineData("A", "A", 100)]
    public void NewCheckout_TwoItemsScanned_ShouldReturnTotal(string itemCode1, string itemCode2, int expectedPrice)
    {
        _checkout.Scan(itemCode1);
        _checkout.Scan(itemCode2);
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }

    [Theory]
    [InlineData("A", 3, 130)]
    [InlineData("B", 2, 45)]
    public void NewCheckout_ThreeItemsScanned_SpecialPriceTriggered_ShouldReturnTotal(string itemCode, int scanCount, int expectedPrice)
    {

        ScanMultiple(_checkout, itemCode, scanCount);

        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }

    [Theory]
    [InlineData("A", 3, "B", 2, 175)]
    public void NewCheckout_FiveItemsScanned_TwoSpecialPricesTriggered_ShouldReturnTotal(string itemCode1,
        int scanCount1, string itemCode2, int scanCount2, int expectedPrice)
    {
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }

    [Theory]
    [InlineData("A", 2, "B", 1, "C", 7, 270)]
    public void NewCheckout_TenItemsScanned_NoSpecialPricesTriggered_ShouldReturnTotal(string itemCode1, int scanCount1,
        string itemCode2, int scanCount2, string itemCode3, int scanCount3, int expectedPrice)
    {
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        ScanMultiple(_checkout, itemCode3, scanCount3);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }

    private static void ScanMultiple(Checkout checkout, string code, int scanCount)
    {
        var count = 0;
        while (count < scanCount)
        {
            checkout.Scan(code);
            count++;
        }
    }
}