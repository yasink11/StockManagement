using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Web.Services;

namespace StockManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly TestDbContext _db;
    private readonly DatabaseHealthService _health;

    public HomeController(TestDbContext db, DatabaseHealthService health)
    {
        _db = db;
        _health = health;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var (ok, message) = await _health.TestAsync(_db, ct);
        ViewBag.DbOk = ok;
        ViewBag.DbMessage = message;
        return View();
    }

    public IActionResult DbError(string? message)
    {
        ViewBag.Message = message;
        return View();
    }

    public IActionResult Error() => View();


    // =========================
    // 📈 SALES CHART
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetSalesChart(CancellationToken ct)
    {
        var data = await _db.Sales
            .AsNoTracking()
            .Where(x => x.Date != null)
            .ToListAsync(ct);

        var result = data
            .GroupBy(x => x.Date!.Value.Month)
            .Select(x => new
            {
                Month = x.Key,
                Total = x.Sum(s => s.Amount)
            })
            .OrderBy(x => x.Month)
            .ToList();

        return Json(result);
    }

    // =========================
    // 📊 PURCHASE CHART
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetPurchaseChart(CancellationToken ct)
    {
        var data = await _db.Purchases
            .AsNoTracking()
            .Where(x => x.Date != null)
            .ToListAsync(ct);

        var result = data
            .GroupBy(x => x.Date!.Value.Month)
            .Select(x => new
            {
                Month = x.Key,
                Total = x.Sum(p => p.Amount)
            })
            .OrderBy(x => x.Month)
            .ToList();

        return Json(result);
    }

    // =========================
    // 💰 PROFIT
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetProfitChart(CancellationToken ct)
    {
        var sales = await _db.Sales.SumAsync(x => (decimal?)x.Amount, ct) ?? 0;
        var purchases = await _db.Purchases.SumAsync(x => (decimal?)x.Amount, ct) ?? 0;

        return Json(new
        {
            Sales = sales,
            Purchases = purchases,
            Profit = sales - purchases
        });
    }
}