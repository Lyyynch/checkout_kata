using CheckoutKata.Tests.Stubs;

namespace CheckoutKata.Tests;

public class CheckoutTests
{
    private readonly SkuRetrievalServiceStub _skuRetrievalService = new ();
    private readonly SkuSpecialRetrievalServiceStub _skuSpecialRetrievalService = new ();
    
    [Fact]
    public void NewCheckout_NoItemsScanned_ShouldReturnZero()
    {
        var checkout = new Checkout(_skuRetrievalService, _skuSpecialRetrievalService);
        var total = checkout.Total;
        Assert.Equal(0, total);
    }

    [Theory]
    [InlineData("A", 50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void NewCheckout_OneItemScanned_ShouldReturnTotal(string itemCode, int itemPrice)
    {
        var checkout = new Checkout(_skuRetrievalService, _skuSpecialRetrievalService);
        checkout.Scan(itemCode);
        var total = checkout.Total;
        Assert.Equal(itemPrice, total);
    }

    [Theory]
    [InlineData("A", "B", 80)]
    [InlineData("B", "C", 50)]
    [InlineData("A", "A", 100)]
    public void NewCheckout_TwoItemsScanned_ShouldReturnTotal(string itemCode1, string itemCode2, int expectedPrice)
    {
        var checkout = new Checkout(_skuRetrievalService, _skuSpecialRetrievalService);
        checkout.Scan(itemCode1);
        checkout.Scan(itemCode2);
        var total = checkout.Total;
        Assert.Equal(expectedPrice, total);
    }

    [Theory]
    [InlineData("A", 3, 130)]
    [InlineData("B", 2, 45)]
    public void NewCheckout_ThreeItemsScanned_SpecialPriceTriggered_ShouldReturnTotal(string itemCode, int scanCount, int expectedPrice)
    {
        var checkout = new Checkout(_skuRetrievalService, _skuSpecialRetrievalService);
        var count = 0;
        while (count < scanCount)
        {
            checkout.Scan(itemCode);
            count++;
        }
        var total = checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
}