namespace CheckoutKata;

public class Checkout
{
    private readonly ISkuRetrievalService _skuRetrievalService;
    private readonly ISkuSpecialRetrievalService _skuSpecialRetrievalService;
    
    public Checkout(ISkuRetrievalService skuRetrievalService, ISkuSpecialRetrievalService skuSpecialRetrievalService)
    {
        _skuRetrievalService = skuRetrievalService;
        _skuSpecialRetrievalService = skuSpecialRetrievalService;
    }
    
    public int Total { get; private set; }
    
    private readonly List<string> _skuCodes = new();

    public void Scan(string code)
    {
        var validSku = _skuRetrievalService.GetSkuByCode(code);
        
        _skuCodes.Add(validSku.Code);
        
        var skuSpecial = _skuSpecialRetrievalService.GetSkuSpecialByCode(code);

        var count = _skuCodes.Count(skuCode => skuCode == code);

        if (skuSpecial != null && count % skuSpecial.Quantity == 0)
        {
            Total += skuSpecial.NewPrice;
        }
        else
        {
            Total += validSku.Price;
        }

    }
}