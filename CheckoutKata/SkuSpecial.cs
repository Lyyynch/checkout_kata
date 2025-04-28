namespace CheckoutKata;

public class SkuSpecial(int quantity, int discount, int? limit = null)
{
    public int GetDiscount(int skuCount, int skuTotal)
    {
        var fullDiscountCount = skuCount - (skuCount % quantity);
        var potentialDiscountCount = fullDiscountCount / quantity;

        if (limit is null or 0)
        {
            return skuTotal - (potentialDiscountCount * discount);
        }
        
        return skuTotal - (Math.Min(potentialDiscountCount, limit.Value) * discount);
    }
}