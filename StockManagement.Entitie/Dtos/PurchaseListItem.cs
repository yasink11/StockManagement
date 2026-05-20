namespace StockManagement.Entities.Dtos;

public class PurchaseListItem
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public double? Quantity { get; set; }
    public double? Price { get; set; }
    public DateTime? Date { get; set; }
    public double? Amount { get; set; }
}
