using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;
using StockManagement.Web.Models;

namespace StockManagement.Web.Controllers;

public class SalesController : Controller
{
    private readonly TestDbContext _db;
    private readonly StockService _stockService;
    private readonly ReportService _reportService;

    public SalesController(TestDbContext db, StockService stockService, ReportService reportService)
    {
        _db = db;
        _stockService = stockService;
        _reportService = reportService;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var data = await _reportService.GetAllSalesAsync(ct);
        return DataSourceLoader.Load(data, loadOptions);
    }

    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadLookupsAsync(ct);
        return View(new SaleCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaleCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync(ct);
            return View(model);
        }

        try
        {
            await _stockService.EnsureStockAvailableAsync(model.ProductId, model.Quantity, ct);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadLookupsAsync(ct);
            return View(model);
        }

        model.DiscountRate = SalesHelper.CalculateDiscountRate(model.ListPrice, model.SalesPrice);
        model.Amount = SalesHelper.CalculateAmount(model.Quantity, model.SalesPrice);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var sale = new Sale
            {
                CustomerId = model.CustomerId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                Listprice = model.ListPrice,
                Salesprice = model.SalesPrice,
                Discountrate = model.DiscountRate,
                Amount = model.Amount,
                Date = model.Date
            };
            _db.Sales.Add(sale);
            await _db.SaveChangesAsync(ct);

            _db.Stocks.Add(new Stock
            {
                ProductId = model.ProductId,
                Quantity = -model.Quantity,
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

    [HttpGet]
    public IActionResult Calculate(double listPrice, double salesPrice, double quantity)
    {
        var discount = SalesHelper.CalculateDiscountRate(listPrice, salesPrice);
        var amount = SalesHelper.CalculateAmount(quantity, salesPrice);
        return Json(new { discountRate = discount, amount });
    }

    [HttpGet]
    public async Task<double> GetStock(int productId, CancellationToken ct)
    {
        return await _stockService.GetCurrentStockAsync(productId, ct);
    }

    private async Task LoadLookupsAsync(CancellationToken ct)
    {
        var customers = await _db.Customers.OrderBy(c => c.Customertitle).ToListAsync(ct);
        ViewBag.CustomerId = new SelectList(
            customers.Select(c => new
            {
                c.Id,
                Display = $"{c.Customertitle} {c.Customernumber}".Trim()
            }),
            "Id", "Display");

        ViewBag.ProductId = new SelectList(
            await _db.Products.OrderBy(p => p.Name).ToListAsync(ct),
            "Id", "Name");
    }
}
