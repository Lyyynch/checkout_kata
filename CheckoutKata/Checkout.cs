namespace CheckoutKata;

public class Checkout
{
    private readonly ISkuRetrievalService _skuRetrievalService;
    
    public Checkout(ISkuRetrievalService skuRetrievalService)
    {
        _skuRetrievalService = skuRetrievalService;
    }

    public int Total
    {
        get
        {
            var total = 0;
            
            foreach (var (_, sku) in _skuCodes)
            {
                total += sku.GetTotal();
            }
            
            return total;
        }
    }

    public int DiscountedTotal
    {
        get
        {
            var discountedTotal = 0;
            
            foreach (var (_, sku) in _skuCodes)
            {
                discountedTotal += sku.GetDiscountedTotal();
            }
            
            return discountedTotal;
        }
    }

    private readonly Dictionary<string, Sku> _skuCodes = new();

    public void Scan(string code)
    {
        if (!_skuCodes.TryGetValue(code, out var validSku))
        {
            validSku = _skuRetrievalService.GetSkuByCode(code);
            _skuCodes.Add(code, validSku);
        }
        
        validSku.Increment();
    }
}