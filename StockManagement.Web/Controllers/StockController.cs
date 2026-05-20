using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;

namespace StockManagement.Web.Controllers;

public class StockController : Controller
{
    private readonly TestDbContext _db;
    private readonly StockService _stockService;

    public StockController(TestDbContext db, StockService stockService)
    {
        _db = db;
        _stockService = stockService;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var query = _db.Stocks.AsNoTracking()
            .Select(s => new
            {
                s.Id,
                s.ProductId,
                ProductName = s.Product != null ? s.Product.Name : null,
                CategoryName = s.Product != null && s.Product.Category != null ? s.Product.Category.Name : null,
                s.Quantity,
                s.Date
            });
        return await DataSourceLoader.LoadAsync(query, loadOptions, ct);
    }

    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadProductsAsync(ct);
        return View(new Stock { Date = DateTime.Now, Quantity = 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Stock model, CancellationToken ct)
    {
        if (model.ProductId is null or <= 0)
            ModelState.AddModelError(nameof(model.ProductId), "Ürün seçiniz.");
        if (model.Quantity is null or <= 0)
            ModelState.AddModelError(nameof(model.Quantity), "Miktar 0'dan büyük olmalıdır.");
        if (!ModelState.IsValid)
        {
            await LoadProductsAsync(ct);
            return View(model);
        }

        model.Date ??= DateTime.Now;
        await _stockService.AddStockMovementAsync(model.ProductId!.Value, model.Quantity!.Value, model.Date, ct);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadProductsAsync(CancellationToken ct)
    {
        var products = await _db.Products.OrderBy(p => p.Name).ToListAsync(ct);
        ViewBag.ProductId = new SelectList(products, "Id", "Name");
    }
}
