namespace StockManagement.Entities.Models;

public class StockEntry
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public DateTime? Date { get; set; }
    public string? ProductName { get; set; }
}
