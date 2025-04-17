namespace CheckoutKata;

public interface ISkuSpecialRetrievalService
{
    SkuSpecial? GetSkuSpecialByCode(string skuCode);
}