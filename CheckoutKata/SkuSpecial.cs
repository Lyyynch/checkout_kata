namespace CheckoutKata;

public abstract class SkuSpecial
{
    private readonly int _quantity;
    
    protected SkuSpecial(int quantity)
    {
        _quantity = quantity;
    }

    protected int Quantity => _quantity;

    public float GetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuTotal = sku.GetTotal();
        
        var fullDiscountCount = skuCount - (skuCount % _quantity);
        var potentialDiscountCount = fullDiscountCount / _quantity;

        if (potentialDiscountCount == 0)
        {
            return skuTotal;
        }

        return OnGetDiscount(sku);
    }
    
    protected abstract float OnGetDiscount(Sku sku);
}

public class FlatDiscountSkuSpecial : SkuSpecial
{
    private readonly int _discount;
    private readonly int? _limit;
    
    public FlatDiscountSkuSpecial(int quantity, int discount, int? limit = null) : base(quantity)
    {
        _discount = discount;
        _limit = limit;
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuTotal = sku.GetTotal();
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        if (_limit is null or 0)
        {
            return skuTotal - (potentialDiscountCount * _discount);
        }
        
        return skuTotal - (Math.Min(potentialDiscountCount, _limit.Value) * _discount);
    }
}

public class BuyXGetYFreeDiscountSkuSpecial : SkuSpecial
{
    private readonly int? _limit;
    
    public BuyXGetYFreeDiscountSkuSpecial(int quantity, int? limit = null) : base(quantity)
    {
        _limit = limit;
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuPrice = sku.Price;
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        if (_limit is { } or 0)
        {
            potentialDiscountCount = Math.Min(potentialDiscountCount, _limit.Value);
        }
        
        var discountedItems = potentialDiscountCount * Quantity;
        var fullPriceItems = skuCount - discountedItems;
        
        var discountedTotal = (discountedItems * skuPrice) - (potentialDiscountCount * skuPrice);
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}

public class PercentageDiscountSkuSpecial : SkuSpecial
{
    private readonly int _discount;
    private readonly int? _limit;
    
    public PercentageDiscountSkuSpecial(int quantity, int discount, int? limit = null) : base(quantity)
    {
        _discount = discount;
        _limit = limit;
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuPrice = sku.Price;
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        var percentDiscount = 1 - (_discount / 100f);
        
        if (_limit is { } or > 0)
        {
            potentialDiscountCount = Math.Min(potentialDiscountCount, _limit.Value);
        }

        var discountedItems = potentialDiscountCount * Quantity;
        var fullPriceItems = skuCount - discountedItems;
        
        var discountedTotal = discountedItems * skuPrice * percentDiscount;
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}