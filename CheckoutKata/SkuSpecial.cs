namespace CheckoutKata;

public interface ISpecialLimits
{
    int? Limit { get; }
}

internal static class SpecialLimitsExtension
{
    public static int GetPotentialDiscountCount(this ISpecialLimits limits, int discountCount)
    {
        if (limits.Limit is { } or 0)
        {
            discountCount = Math.Min(discountCount, limits.Limit.Value);
        }
        
        return discountCount;
    }
}

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

public class FlatDiscountSkuSpecial : SkuSpecial, ISpecialLimits
{
    private readonly int _discount;
    private readonly int? _limit;
    
    public int? Limit => _limit;
    
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

public class BuyXGetYFreeDiscountSkuSpecial : SkuSpecial, ISpecialLimits
{
    private readonly int? _limit;
    
    public int? Limit => _limit;
    
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

        potentialDiscountCount = this.GetPotentialDiscountCount(potentialDiscountCount);
        
        var discountedItems = potentialDiscountCount * Quantity;
        var fullPriceItems = skuCount - discountedItems;
        
        var discountedTotal = (discountedItems * skuPrice) - (potentialDiscountCount * skuPrice);
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}

public class PercentageDiscountSkuSpecial : SkuSpecial, ISpecialLimits
{
    private readonly int _discount;
    private readonly int? _limit;
    
    public int? Limit => _limit;
    
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
        
        potentialDiscountCount = this.GetPotentialDiscountCount(potentialDiscountCount);

        var discountedItems = potentialDiscountCount * Quantity;
        var fullPriceItems = skuCount - discountedItems;
        
        var discountedTotal = discountedItems * skuPrice * percentDiscount;
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}