using Microsoft.EntityFrameworkCore;
using StockManagement.Data.Entities;

namespace StockManagement.Data;

public class ReportService
{
    private readonly TestDbContext _db;

    public ReportService(TestDbContext db) => _db = db;

    public async Task<List<PurchaseListDto>> GetAllPurchasesAsync(CancellationToken ct = default)
    {
        return await _db.Set<PurchaseListDto>()
            .FromSqlRaw("EXEC SPGetAllPurchases")
            .ToListAsync(ct);
    }

    public async Task<List<SalesListDto>> GetAllSalesAsync(CancellationToken ct = default)
    {
        return await _db.Set<SalesListDto>()
            .FromSqlRaw("EXEC SPGetAllSales")
            .ToListAsync(ct);
    }

    public async Task<List<SalesReportDto>> GetSalesDetailReportAsync(
     DateTime? startDate,
     DateTime? endDate,
     CancellationToken ct = default)
    {
        var data = await _db.Database
            .SqlQueryRaw<SalesReportDto>("EXEC SPReportGetAllSalesDetail")
            .ToListAsync(ct);

        if (startDate.HasValue)
            data = data.Where(x => x.Date >= startDate.Value).ToList();

        if (endDate.HasValue)
            data = data.Where(x => x.Date <= endDate.Value.Date.AddDays(1).AddTicks(-1)).ToList();

        return data
            .OrderByDescending(x => x.TotalQuantity)
            .ToList();
    }
}
