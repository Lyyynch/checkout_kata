namespace CheckoutKata;

public class Checkout
{
    private readonly ISkuRetrievalService _skuRetrievalService;
    
    public Checkout(ISkuRetrievalService skuRetrievalService)
    {
        _skuRetrievalService = skuRetrievalService;
    }
    
    public int Total { get; private set; }

    public void Scan(string code)
    {
        var validSku = _skuRetrievalService.GetSkuByCode(code);

        Total += validSku.Price;
    }
}