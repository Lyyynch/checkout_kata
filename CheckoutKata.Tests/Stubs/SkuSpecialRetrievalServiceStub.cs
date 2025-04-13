namespace CheckoutKata.Tests.Stubs;

public class SkuSpecialRetrievalServiceStub : ISkuSpecialRetrievalService
{
    private readonly List<SkuSpecial> _itemList = 
    [
        new SkuSpecial("A", 3, 30),
        new SkuSpecial("B", 2, 15)
    ];

    public List<SkuSpecial> GetSkuSpecials()
    {
        return _itemList;
    }

    public SkuSpecial? GetSkuSpecialByCode(string skuCode)
    {
        return _itemList.FirstOrDefault(skuSpecial => skuSpecial?.Code == skuCode, null);
    }
}