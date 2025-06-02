namespace CheckoutKata;

public enum SpecialType
{
    Flat,
    BuyXGetYFree,
    Percentage
}

public class SkuSpecial
{
    private readonly int _quantity;
    private readonly int _discount;
    private readonly int? _limit;
    private readonly SpecialType _type;
    private readonly Sku _sku;
    
    public SkuSpecial(Sku sku, SpecialType type, int quantity, int discount, int? limit = null)
    {
        _sku = sku;
        _type = type;
        _quantity = quantity;
        _discount = discount;
        _limit = limit;
    }

    public float GetDiscount()
    {
        var skuCount = _sku.GetCount();
        var skuTotal = _sku.GetTotal();
        var skuPrice = _sku.Price;
        
        var fullDiscountCount = skuCount - (skuCount % _quantity);
        var potentialDiscountCount = fullDiscountCount / _quantity;

        if (potentialDiscountCount == 0)
        {
            return skuTotal;
        }

        return _type switch
        {
            SpecialType.Flat => GetFlatDiscount(skuTotal, potentialDiscountCount),
            SpecialType.BuyXGetYFree => GetBuyXGetYFreeDiscount(skuTotal, potentialDiscountCount, skuPrice),
            SpecialType.Percentage => GetPercentageDiscount(potentialDiscountCount, skuCount, skuPrice),
            _ => skuTotal
        };
    }

    private float GetFlatDiscount(float skuTotal, int potentialDiscountCount)
    {
        if (_limit is null or 0)
        {
            return skuTotal - (potentialDiscountCount * _discount);
        }
        
        return skuTotal - (Math.Min(potentialDiscountCount, _limit.Value) * _discount);
    }

    private float GetBuyXGetYFreeDiscount(float skuTotal, int potentialDiscountCount, int skuPrice)
    {
        var soItsDifferent = true;
        
        if (!soItsDifferent)
            return skuTotal;
        
        if (_limit is null or 0)
        {
            return skuTotal - (potentialDiscountCount * skuPrice);
        }
        
        return skuTotal - (Math.Min(potentialDiscountCount, _limit.Value) * skuPrice);
    }

    private float GetPercentageDiscount(int potentialDiscountCount, int skuCount, int skuPrice)
    {
        float percentDiscount = 1 - (_discount / 100f);
        
        if (_limit is { } or > 0)
        {
            potentialDiscountCount = Math.Min(potentialDiscountCount, _limit.Value);
        }

        int discountedItems = potentialDiscountCount * _quantity;
        int fullPriceItems = skuCount - discountedItems;
        
        float discountedTotal = discountedItems * skuPrice * percentDiscount;
        float fullPriceTotal = fullPriceItems * skuPrice;
        
        return discountedTotal + fullPriceTotal;
    }
}