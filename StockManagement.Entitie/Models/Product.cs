namespace StockManagement.Entities.Models;

public class Product
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string? Name { get; set; }
    public string? ImageSrc { get; set; }
    public double? SalesPrice { get; set; }
    public string? CategoryName { get; set; }
}
