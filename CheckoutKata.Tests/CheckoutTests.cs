using CheckoutKata.Tests.Stubs;

namespace CheckoutKata.Tests;

public class CheckoutTests
{
    private readonly Checkout _checkout = new (new SkuRetrievalServiceStub());
    
    [Fact]
    public void NewCheckout_NoItemsScanned_ShouldReturnZero()
    {
        var total = _checkout.Total;
        Assert.Equal(0, total);
    }
    
    [Fact]
    public void NewCheckout_NoItemsScanned_DiscountTotalShouldBeZero()
    {
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(0, discountedTotal);
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
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    public void NewCheckout_OneItemScanned_DiscountedTotalShouldEqualTotal(string itemCode)
    {
        _checkout.Scan(itemCode);
        var total = _checkout.Total;
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(discountedTotal, total);
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
    [InlineData("A", "B")]
    [InlineData("B", "C")]
    [InlineData("A", "A")]
    public void NewCheckout_TwoItemsScanned_DiscountedTotalShouldEqualTotal(string itemCode1, string itemCode2)
    {
        _checkout.Scan(itemCode1);
        _checkout.Scan(itemCode2);
        var total = _checkout.Total;
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(discountedTotal, total);
    }

    [Theory]
    [InlineData("A", 3, 150)]
    [InlineData("B", 2, 60)]
    public void NewCheckout_ThreeItemsScanned_SpecialPriceTriggered_ShouldReturnTotalPreDiscount(string itemCode, 
        int scanCount, int expectedPrice)
    {

        ScanMultiple(_checkout, itemCode, scanCount);

        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Theory]
    [InlineData("A", 3, 130)]
    [InlineData("B", 2, 45)]
    public void NewCheckout_ThreeItemsScanned_SpecialPriceTriggered_ShouldReturnTotalPostDiscount(string itemCode, 
        int scanCount, int expectedPrice)
    {
        ScanMultiple(_checkout, itemCode, scanCount);

        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(expectedPrice, discountedTotal);
    }

    [Fact]
    public void NewCheckout_FiveItemsScanned_TwoSpecialPricesTriggered_ShouldReturnTotalPreDiscount()
    {
        const string itemCode1 = "A";
        const int scanCount1 = 3;
        const string itemCode2 = "B";
        const int scanCount2 = 2;
        const int expectedPrice = 210;
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Fact]
    public void NewCheckout_FiveItemsScanned_TwoSpecialPricesTriggered_ShouldReturnTotalPostDiscount()
    {
        const string itemCode1 = "A";
        const int scanCount1 = 3;
        const string itemCode2 = "B";
        const int scanCount2 = 2;
        const int expectedPrice = 175;
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(expectedPrice, discountedTotal);
    }

    [Theory]
    [InlineData("A", 2, "B", 1, "C", 7, 270)]
    [InlineData("F", 1, "G", 3, "D", 6, 230)]
    public void NewCheckout_TenItemsScanned_NoSpecialPricesTriggered_ShouldReturnTotal(string itemCode1, int scanCount1,
        string itemCode2, int scanCount2, string itemCode3, int scanCount3, int expectedPrice)
    {
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        ScanMultiple(_checkout, itemCode3, scanCount3);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Theory]
    [InlineData("A", 2, "B", 1, "C", 7)]
    [InlineData("F", 1, "G", 3, "D", 6)]
    public void NewCheckout_TenItemsScanned_NoSpecialPricesTriggered_DiscountTotalShouldEqualTotal(string itemCode1, 
        int scanCount1, string itemCode2, int scanCount2, string itemCode3, int scanCount3)
    {
        
        ScanMultiple(_checkout, itemCode1, scanCount1);
        
        ScanMultiple(_checkout, itemCode2, scanCount2);
        
        ScanMultiple(_checkout, itemCode3, scanCount3);
        
        var total = _checkout.Total;
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(discountedTotal, total);
    }

    [Theory]
    [InlineData("E", 4, 180)]
    [InlineData("G", 4, 120)]
    public void NewCheckout_FourItemsScanned_BuyThreeGetOneFreeTriggered_ShouldReturnTotalPreDiscount(string itemCode,
        int scanCount, int expectedPrice)
    {
        ScanMultiple(_checkout, itemCode, scanCount);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Theory]
    [InlineData("E", 4, 135)]
    [InlineData("G", 4, 90)]
    public void NewCheckout_FourItemsScanned_BuyThreeGetOneFreeTriggered_ShouldReturnTotalPostDiscount(string itemCode,
        int scanCount, int expectedPrice)
    {
        ScanMultiple(_checkout, itemCode, scanCount);
        
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(expectedPrice, discountedTotal);
    }

    [Fact]
    public void NewCheckout_TwoItemsScanned_BuyTwoGetPercentageOff_ShouldReturnTotalPreDiscount()
    {
        const string itemCode = "F";
        const int scanCount = 2;
        const int expectedPrice = 100;
        
        ScanMultiple(_checkout, itemCode, scanCount);
        
        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Fact]
    public void NewCheckout_TwoItemsScanned_BuyTwoGetPercentageOff_ShouldReturnTotalPostDiscount()
    {
        const string itemCode = "F";
        const int scanCount = 2;
        const int expectedPrice = 70;
        
        ScanMultiple(_checkout, itemCode, scanCount);
        
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(expectedPrice, discountedTotal);
    }

    [Theory]
    [InlineData("G", 8, 240)]
    [InlineData("H", 16, 960)]
    public void NewCheckout_EightItemsScanned_BuyThreeGetOneFreeTriggeredOnce_ShouldReturnTotalPreDiscount(
        string itemCode, int scanCount, int expectedPrice)
    {
        ScanMultiple(_checkout, itemCode, scanCount);

        var total = _checkout.Total;
        Assert.Equal(expectedPrice, total);
    }
    
    [Theory]
    [InlineData("G", 8, 210)]
    [InlineData("H", 16, 780)]
    public void NewCheckout_EightItemsScanned_BuyThreeGetOneFreeTriggeredOnce_ShouldReturnTotalPostDiscount(
        string itemCode, int scanCount, int expectedPrice)
    {
        ScanMultiple(_checkout, itemCode, scanCount);
        
        var discountedTotal = _checkout.DiscountedTotal;
        Assert.Equal(expectedPrice, discountedTotal);
    }

    [Fact]
    public void NewCheckout_EightItemsScanned_ShouldReturnTotalBeforeDiscount()
    {
        const string itemCode1 = "A";
        const string itemCode2 = "C";
        const int scanCount = 4;
        const int expectedPrice = 280;
        
        ScanMultiple(_checkout, itemCode1, scanCount);
        ScanMultiple(_checkout, itemCode2, scanCount);
        
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