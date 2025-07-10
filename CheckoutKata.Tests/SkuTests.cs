namespace CheckoutKata.Tests;

public class SkuTests
{
    private readonly Sku _skuFlatSpecial = new("H", 20, new FlatDiscountSkuSpecial(4, 20 , 3));
    private readonly Sku _skuPercentSpecial = new("I", 20, new PercentageDiscountSkuSpecial(2, 50));
    
    [Fact]
    public void NewSku_IncrementCountByOne_ShouldReturnCountOfOne()
    {
        _skuFlatSpecial.Increment();
        
        var count = _skuFlatSpecial.GetCount();
        Assert.Equal(1, count);
    }

    [Fact]
    public void NewSku_IncrementCountByTwo_DecrementCountByTwo_ShouldReturnCountOfOne()
    {
        IncrementSku(_skuFlatSpecial, 2);
        
        DecrementSku(_skuFlatSpecial, 1);
        
        var count = _skuFlatSpecial.GetCount();
        Assert.Equal(1, count);
    }

    [Fact]
    public void NewSku_IncrementCountByThree_ShouldReturnTotalOfSixty()
    {
        IncrementSku(_skuFlatSpecial, 3);
        
        var total = _skuFlatSpecial.GetTotal();
        Assert.Equal(60, total);
    }

    [Fact]
    public void NewSku_IncrementCountByThree_ShouldReturnDiscountTotalOfSixty()
    {
        IncrementSku(_skuFlatSpecial, 3);
        
        var total = _skuFlatSpecial.GetDiscountedTotal();
        Assert.Equal(60, total);
    }

    [Fact]
    public void NewSku_IncrementCountByTwelve_ShouldReturnTotalOf240()
    {
        IncrementSku(_skuFlatSpecial, 12);

        var total = _skuFlatSpecial.GetTotal();
        Assert.Equal(240, total);
    }
    
    [Fact]
    public void NewSku_IncrementCountByTwelve_ShouldReturnDiscountedTotalOf180()
    {
        IncrementSku(_skuFlatSpecial, 12);

        var total = _skuFlatSpecial.GetDiscountedTotal();
        Assert.Equal(180, total);
    }
    
    [Fact]
    public void NewSku_IncrementCountByTen_ShouldReturnTotalOf200()
    {
        IncrementSku(_skuPercentSpecial, 10);
        
        var total = _skuPercentSpecial.GetTotal();
        Assert.Equal(200, total);
    }

    [Fact]
    public void NewSku_IncrementCountByTen_ShouldReturnDiscountedTotalOf100()
    {
        IncrementSku(_skuPercentSpecial, 10);
        
        var total = _skuPercentSpecial.GetDiscountedTotal();
        Assert.Equal(100, total);
    }

    private static void IncrementSku(Sku sku, int count)
    {
        var incCount = 0;
        while (incCount < count)
        {
            sku.Increment();
            incCount++;
        }
    }

    private static void DecrementSku(Sku sku, int count)
    {
        var decCount = 0;
        while (decCount < count)
        {
            sku.Decrement();
            decCount++;
        }
    }
}