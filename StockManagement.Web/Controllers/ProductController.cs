using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;

namespace StockManagement.Web.Controllers;

public class ProductController : Controller
{
    private readonly TestDbContext _db;

    public ProductController(TestDbContext db) => _db = db;

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var query = _db.Products.AsNoTracking()
            .Select(p => new
            {
                p.Id,
                p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                p.Name,
                p.ImageSrc,
                p.Salesprice
            });
        return await DataSourceLoader.LoadAsync(query, loadOptions, ct);
    }

    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadCategoriesAsync(ct);
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            ModelState.AddModelError(nameof(model.Name), "Ürün adı zorunludur.");
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(ct);
            return View(model);
        }

        _db.Products.Add(model);
        await _db.SaveChangesAsync(ct);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await _db.Products.FindAsync([id], ct);
        if (entity == null)
            return NotFound();
        await LoadCategoriesAsync(ct);
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product model, CancellationToken ct)
    {
        if (id != model.Id)
            return BadRequest();
        if (string.IsNullOrWhiteSpace(model.Name))
            ModelState.AddModelError(nameof(model.Name), "Ürün adı zorunludur.");
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(ct);
            return View(model);
        }

        _db.Update(model);
        await _db.SaveChangesAsync(ct);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await _db.Products.FindAsync([id], ct);
        if (entity != null)
        {
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> GetProductInfo(int id, CancellationToken ct)
    {
        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (product == null)
            return NotFound();

        return Json(new
        {
            product.Id,
            product.Name,
            ListPrice = product.Salesprice ?? 0,
            SalesPrice = product.Salesprice ?? 0
        });
    }

    private async Task LoadCategoriesAsync(CancellationToken ct)
    {
        var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync(ct);
        ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
    }
}
