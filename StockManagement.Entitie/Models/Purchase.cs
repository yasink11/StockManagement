namespace StockManagement.Entities.Models;

public class Purchase
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public double? Price { get; set; }
    public double? Amount { get; set; }
    public DateTime? Date { get; set; }
    public int? CustomerId { get; set; }
}
