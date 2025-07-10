namespace CheckoutKata;

public abstract class SkuSpecial
{
    private readonly int _quantity;
    private readonly int _discount;
    private readonly int? _limit;
    
    protected SkuSpecial(int quantity, int discount, int? limit = null)
    {
        _quantity = quantity;
        _discount = discount;
        _limit = limit;
    }

    protected int Quantity => _quantity;
    protected int Discount => _discount;
    protected int? Limit => _limit;

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
    public FlatDiscountSkuSpecial(int quantity, int discount, int? limit = null) : base(quantity, discount, limit)
    {
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuTotal = sku.GetTotal();
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        if (Limit is null or 0)
        {
            return skuTotal - (potentialDiscountCount * Discount);
        }
        
        return skuTotal - (Math.Min(potentialDiscountCount, Limit.Value) * Discount);
    }
}

public class BuyXGetYFreeDiscountSkuSpecial : SkuSpecial
{
    public BuyXGetYFreeDiscountSkuSpecial(int quantity, int discount, int? limit = null) : base(quantity, discount, limit)
    {
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuPrice = sku.Price;
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        if (Limit is { } or 0)
        {
            potentialDiscountCount = Math.Min(potentialDiscountCount, Limit.Value);
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
    public PercentageDiscountSkuSpecial(int quantity, int discount, int? limit = null) : base(quantity, discount, limit)
    {
    }

    protected override float OnGetDiscount(Sku sku)
    {
        var skuCount = sku.GetCount();
        var skuPrice = sku.Price;
        
        var fullDiscountCount = skuCount - (skuCount % Quantity);
        var potentialDiscountCount = fullDiscountCount / Quantity;
        
        var percentDiscount = 1 - (Discount / 100f);
        
        if (Limit is { } or > 0)
        {
            potentialDiscountCount = Math.Min(potentialDiscountCount, Limit.Value);
        }

        var discountedItems = potentialDiscountCount * Quantity;
        var fullPriceItems = skuCount - discountedItems;
        
        var discountedTotal = discountedItems * skuPrice * percentDiscount;
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}