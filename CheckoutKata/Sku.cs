namespace CheckoutKata;

public class Sku
{
    private readonly int _price;
    private readonly SkuSpecial? _skuSpecial = null;
    private int _count;

    public Sku(string code, int price, SkuSpecial? skuSpecial = null)
    {
        Code = code;
        _price = price;
        _skuSpecial = skuSpecial;
    }
    
    public string Code { get; }

    public void Increment()
    {
        _count++;
    }

    public void Decrement()
    {
        _count--;
    }

    public int GetCount()
    {
        return _count;
    }

    public int GetTotal()
    {
        return _count * _price;
    }

    public int GetDiscountedTotal()
    {
        var total = GetTotal();

        return _skuSpecial?.GetDiscount(_count, total) ?? total;
    }
}