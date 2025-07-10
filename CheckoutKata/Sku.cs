namespace CheckoutKata;

public class Sku
{
    private readonly int _price;
    private SkuSpecial? _skuSpecial = null;
    private int _count;

    public Sku(string code, int price)
    {
        Code = code;
        _price = price;
    }
    
    public string Code { get; }
    public int Price => _price;

    public void CreateSpecial(SpecialType type, int quantity, int discount, int? limit = null)
    {
        _skuSpecial = new SkuSpecial(this, type, quantity, discount, limit);
    }

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

        return _skuSpecial?.GetDiscount() ?? total;
    }
}