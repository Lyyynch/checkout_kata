namespace CheckoutKata;

public interface ISkuRetrievalService
{
    Sku GetSkuByCode(string skuCode);
}