using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;
using StockManagement.Web.Models;

namespace StockManagement.Web.Controllers;

public class PurchaseController : Controller
{
    private readonly TestDbContext _db;
    private readonly StockService _stockService;
    private readonly ReportService _reportService;

    public PurchaseController(TestDbContext db, StockService stockService, ReportService reportService)
    {
        _db = db;
        _stockService = stockService;
        _reportService = reportService;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var data = await _reportService.GetAllPurchasesAsync(ct);
        return DataSourceLoader.Load(data, loadOptions);
    }

    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadLookupsAsync(ct);
        return View(new PurchaseCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync(ct);
            return View(model);
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var purchase = new Purchase
            {
                CustomerId = model.CustomerId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                Price = model.Price,
                Amount = model.Quantity * model.Price,
                Date = model.Date
            };
            _db.Purchases.Add(purchase);
            await _db.SaveChangesAsync(ct);

            _db.Stocks.Add(new Stock
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                Date = model.Date
            });
            await _db.SaveChangesAsync(ct);

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadLookupsAsync(CancellationToken ct)
    {
        ViewBag.CustomerId = new SelectList(
            await _db.Customers.OrderBy(c => c.Customertitle).ToListAsync(ct),
            "Id", "Customertitle");
        ViewBag.ProductId = new SelectList(
            await _db.Products.OrderBy(p => p.Name).ToListAsync(ct),
            "Id", "Name");
    }
}
