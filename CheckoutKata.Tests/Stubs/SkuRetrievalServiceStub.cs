namespace CheckoutKata.Tests.Stubs;

public class SkuRetrievalServiceStub : ISkuRetrievalService
{
    private readonly List<Sku> _defaultItemList =
    [
        new Sku("A", 50, new SkuSpecial(3, 20)),
        new Sku("B", 30, new SkuSpecial(2, 15)),
        new Sku("C", 20),
        new Sku("D", 15),
        new Sku("E", 45, new SkuSpecial(4, 45)),
        new Sku("F", 50, new SkuSpecial(2, 30)),
        new Sku("G", 30, new SkuSpecial(4, 30, 1)),
        new Sku("H", 60, new SkuSpecial(4, 60, 3))
    ];
    
    public Sku GetSkuByCode(string skuCode)
    {
        return _defaultItemList.First(sku => sku.Code == skuCode);
    }
}