namespace StockManagement.Data.Entities;

public partial class Purchase
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public double? Price { get; set; }
    public double? Amount { get; set; }
    public DateTime? Date { get; set; }
    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Product? Product { get; set; }
}
