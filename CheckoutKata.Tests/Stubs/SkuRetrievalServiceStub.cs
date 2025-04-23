namespace CheckoutKata.Tests.Stubs;

public class SkuRetrievalServiceStub : ISkuRetrievalService
{
    private readonly List<Sku> _defaultItemList =
    [
        new Sku("A", 50, new SkuSpecial(3, 20)),
        new Sku("B", 30, new SkuSpecial(2, 15)),
        new Sku("C", 20, null),
        new Sku("D", 15, null)
    ];
    
    public Sku GetSkuByCode(string skuCode)
    {
        return _defaultItemList.First(sku => sku.Code == skuCode);
    }
}