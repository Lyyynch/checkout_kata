namespace CheckoutKata;

public record Sku(string Code, int Price, SkuSpecial? Special = null);