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
            return _skuCodes.Values.Sum(sku => sku.GetTotal());
        }
    }

    public int DiscountedTotal
    {
        get
        {
            return _skuCodes.Values.Sum(sku => sku.GetDiscountedTotal());
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