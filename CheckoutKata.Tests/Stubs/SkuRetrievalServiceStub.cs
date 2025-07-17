namespace CheckoutKata.Tests.Stubs;

public class SkuRetrievalServiceStub : ISkuRetrievalService
{
    private readonly List<Sku> _skuList =
    [
        new("A", 50, new FlatDiscountSkuSpecial(3, 20)),
        new("B", 30, new FlatDiscountSkuSpecial(2, 15)),
        new("C", 20),
        new("D", 15),
        new("E", 45, new BuyXGetYFreeDiscountSkuSpecial(4)),
        new("F", 50, new PercentageDiscountSkuSpecial(2, 30)),
        new("G", 30, new BuyXGetYFreeDiscountSkuSpecial(4, 1)),
        new("H", 60, new FlatDiscountSkuSpecial(4, 60, 3))
    ];
    
    public Sku GetSkuByCode(string skuCode)
    {
        return _skuList.First(sku => sku.Code == skuCode);
    }
}