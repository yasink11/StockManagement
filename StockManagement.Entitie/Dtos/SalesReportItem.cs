namespace StockManagement.Entities.Dtos;

public class SalesReportItem
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public double? Quantity { get; set; }
    public double? SalesPrice { get; set; }
    public double? DiscountRate { get; set; }
    public DateTime? Date { get; set; }
    public double? TotalQuantity { get; set; }
}
