namespace CheckoutKata;

public class Sku
{
    private readonly int _price;
    private readonly SkuSpecial? _skuSpecial;
    private int _count;

    public Sku(string code, int price, SkuSpecial? skuSpecial = null)
    {
        Code = code;
        _price = price;
        _skuSpecial = skuSpecial;
    }
    
    public string Code { get; }
    public int Price => _price;
    
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

    public float GetTotal()
    {
        return _count * _price;
    }

    public float GetDiscountedTotal()
    {
        var total = GetTotal();

        return _skuSpecial?.GetDiscount(this) ?? total;
    }
}