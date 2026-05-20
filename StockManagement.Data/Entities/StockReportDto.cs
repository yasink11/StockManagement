using Microsoft.EntityFrameworkCore;

namespace StockManagement.Data.Entities;

[Keyless]
public class StockReportDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? CategoryName { get; set; }
    public double TotalQuantity { get; set; }
}
