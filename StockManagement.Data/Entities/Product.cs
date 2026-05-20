namespace StockManagement.Data.Entities;

public partial class Product
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageSrc { get; set; }
    public double? Salesprice { get; set; }

    public virtual Category? Category { get; set; }
    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
