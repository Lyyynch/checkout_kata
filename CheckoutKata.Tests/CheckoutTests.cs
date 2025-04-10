using CheckoutKata.Tests.Stubs;

namespace CheckoutKata.Tests;

public class CheckoutTests
{
    private readonly SkuRetrievalServiceStub _skuRetrievalService = new ();
    
    [Fact]
    public void NewCheckout_NoItemsScanned_ShouldReturnZero()
    {
        var checkout = new Checkout(_skuRetrievalService);
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
        var checkout = new Checkout(_skuRetrievalService);
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
        var checkout = new Checkout(_skuRetrievalService);
        checkout.Scan(itemCode1);
        checkout.Scan(itemCode2);
        var total = checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
}