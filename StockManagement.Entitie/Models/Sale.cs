namespace StockManagement.Entities.Models;

public class Sale
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public double? SalesPrice { get; set; }
    public DateTime? Date { get; set; }
    public double? Amount { get; set; }
    public int? CustomerId { get; set; }
    public double? ListPrice { get; set; }
    public double? DiscountRate { get; set; }
}
