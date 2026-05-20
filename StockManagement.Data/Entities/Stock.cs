namespace StockManagement.Data.Entities;

public partial class Stock
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public DateTime? Date { get; set; }

    public virtual Product? Product { get; set; }
}
