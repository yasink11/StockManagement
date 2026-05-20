namespace StockManagement.Data.Entities;

public partial class Sale
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public double? Quantity { get; set; }
    public double? Salesprice { get; set; }
    public DateTime? Date { get; set; }
    public double? Amount { get; set; }
    public int? CustomerId { get; set; }
    public double? Listprice { get; set; }
    public double? Discountrate { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Product? Product { get; set; }
}
