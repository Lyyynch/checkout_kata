namespace CheckoutKata;

public class Checkout
{
    private readonly ISkuRetrievalService _skuRetrievalService;
    
    public Checkout(ISkuRetrievalService skuRetrievalService)
    {
        _skuRetrievalService = skuRetrievalService;
    }
    
    public int Total { get; private set; }
    
    private readonly List<string> _skuCodes = new();

    public void Scan(string code)
    {
        var validSku = _skuRetrievalService.GetSkuByCode(code);
        
        _skuCodes.Add(validSku.Code);

        var count = _skuCodes.Count(skuCode => skuCode == code);
        
        Total += validSku.Price;

        if (validSku.Special != null && count % validSku.Special.Quantity == 0)
        {
            Total -= validSku.Special.Discount;
        }
    }
}