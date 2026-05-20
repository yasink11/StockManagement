namespace StockManagement.Data;

public static class SalesHelper
{
    public static double CalculateDiscountRate(double? listPrice, double? salesPrice)
    {
        if (listPrice is null or <= 0 || salesPrice is null)
            return 0;

        return Math.Round(((listPrice.Value - salesPrice.Value) / listPrice.Value) * 100, 2);
    }

    public static double CalculateAmount(double? quantity, double? salesPrice)
    {
        if (quantity is null || salesPrice is null)
            return 0;

        return Math.Round(quantity.Value * salesPrice.Value, 2);
    }
}
