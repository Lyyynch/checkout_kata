namespace CheckoutKata.Tests.Stubs;

public class SkuRetrievalServiceStub : ISkuRetrievalService
{
    private readonly List<Sku> _defaultItemList =
    [
        new Sku("A", 50),
        new Sku("B", 30),
        new Sku("C", 20),
        new Sku("D", 15)
    ];
    
    public Sku GetSkuByCode(string skuCode)
    {
        return _defaultItemList.First(sku => sku.Code == skuCode);
    }
}