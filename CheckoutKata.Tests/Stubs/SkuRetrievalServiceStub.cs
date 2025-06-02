using static CheckoutKata.SpecialType;

namespace CheckoutKata.Tests.Stubs;

public class SkuRetrievalServiceStub : ISkuRetrievalService
{
    private readonly List<Sku> _skuList =
    [
        new("A", 50),
        new("B", 30),
        new("C", 20),
        new("D", 15),
        new("E", 45),
        new("F", 50),
        new("G", 30),
        new("H", 60)
    ];
    
    private readonly List<List<int?>?> _skuSpecialList =
    [
        [0, 3, 20],
        [0, 2, 15],
        [],
        [],
        [1, 4, 45],
        [2, 2, 30],
        [1, 4, 30, 1],
        [0, 4, 60, 3]
    ];

    public SkuRetrievalServiceStub()
    {
        for (var i = 0; i < _skuList.Count; i++)
        {
            if (_skuSpecialList[i]?.Count >= 3)
            {
                var specialArgs = _skuSpecialList[i];
                _skuList[i].CreateSpecial(
                    (SpecialType)specialArgs[0]!.Value, 
                    specialArgs[1]!.Value, 
                    specialArgs[2]!.Value, 
                    specialArgs.Count > 3 ? specialArgs[3] : null
                );
            }
        }
    }
    
    public Sku GetSkuByCode(string skuCode)
    {
        return _skuList.First(sku => sku.Code == skuCode);
    }
}