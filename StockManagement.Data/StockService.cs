using Microsoft.EntityFrameworkCore;
using StockManagement.Data.Entities;

namespace StockManagement.Data;

public class StockService
{
    private readonly TestDbContext _db;

    public StockService(TestDbContext db) => _db = db;

    public async Task<double> GetCurrentStockAsync(int productId, CancellationToken ct = default)
    {
        return await _db.Stocks
            .Where(s => s.ProductId == productId)
            .SumAsync(s => s.Quantity ?? 0, ct);
    }

    public async Task EnsureStockAvailableAsync(int productId, double requestedQty, CancellationToken ct = default)
    {
        var current = await GetCurrentStockAsync(productId, ct);
        if (current - requestedQty < 0)
            throw new InvalidOperationException($"Yetersiz stok. Mevcut stok: {current}");
    }

    public async Task AddStockMovementAsync(int productId, double quantity, DateTime? date = null, CancellationToken ct = default)
    {
        _db.Stocks.Add(new Stock
        {
            ProductId = productId,
            Quantity = quantity,
            Date = date ?? DateTime.Now
        });
        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<Stock>> GetAllWithProductAsync(CancellationToken ct = default)
    {
        return await _db.Stocks
            .Include(s => s.Product)
            .ThenInclude(p => p!.Category)
            .OrderByDescending(s => s.Date)
            .ToListAsync(ct);
    }

    public async Task<List<StockReportDto>> GetStockReportAsync(CancellationToken ct = default)
    {
        return await (
            from s in _db.Stocks
            join p in _db.Products on s.ProductId equals p.Id
            join c in _db.Categories on p.CategoryId equals c.Id into catJoin
            from c in catJoin.DefaultIfEmpty()
            where s.ProductId != null
            group s by new { ProductId = p.Id, p.Name, CategoryName = c != null ? c.Name : null } into g
            select new StockReportDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                CategoryName = g.Key.CategoryName,
                TotalQuantity = g.Sum(x => x.Quantity ?? 0)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToListAsync(ct);
    }
}
