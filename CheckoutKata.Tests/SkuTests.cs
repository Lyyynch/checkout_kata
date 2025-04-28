namespace CheckoutKata.Tests;

public class SkuTests
{
    private readonly Sku _sku = new Sku("H", 20, new SkuSpecial(4, 20, 3));
    
    [Fact]
    public void NewSku_IncrementCountByOne_ShouldReturnCountOfOne()
    {
        _sku.Increment();
        
        var count = _sku.GetCount();
        Assert.Equal(1, count);
    }

    [Fact]
    public void NewSku_IncrementCountByTwo_DecrementCountByTwo_ShouldReturnCountOfOne()
    {
        IncrementSku(_sku, 2);
        
        DecrementSku(_sku, 1);
        
        var count = _sku.GetCount();
        Assert.Equal(1, count);
    }

    [Fact]
    public void NewSku_IncrementCountByThree_ShouldReturnTotalOfSixty()
    {
        IncrementSku(_sku, 3);
        
        var total = _sku.GetTotal();
        Assert.Equal(60, total);
    }

    [Fact]
    public void NewSku_IncrementCountByThree_ShouldReturnDiscountTotalOfSixty()
    {
        IncrementSku(_sku, 3);
        
        var total = _sku.GetDiscountedTotal();
        Assert.Equal(60, total);
    }

    [Fact]
    public void NewSku_IncrementCountByTwelve_ShouldReturnTotalOf240()
    {
        IncrementSku(_sku, 12);

        var total = _sku.GetTotal();
        Assert.Equal(240, total);
    }
    
    [Fact]
    public void NewSku_IncrementCountByTwelve_ShouldReturnDiscountedTotalOf180()
    {
        IncrementSku(_sku, 12);

        var total = _sku.GetDiscountedTotal();
        Assert.Equal(180, total);
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