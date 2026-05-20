using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;

namespace StockManagement.Web.Controllers;

public class CategoryController : Controller
{
    private readonly TestDbContext _db;

    public CategoryController(TestDbContext db) => _db = db;

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var query = _db.Categories.AsNoTracking();
        return await DataSourceLoader.LoadAsync(query, loadOptions, ct);
    }

    public IActionResult Create() => View(new Category());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            ModelState.AddModelError(nameof(model.Name), "Kategori adı zorunludur.");
        if (!ModelState.IsValid)
            return View(model);

        _db.Categories.Add(model);
        await _db.SaveChangesAsync(ct);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await _db.Categories.FindAsync([id], ct);
        if (entity == null)
            return NotFound();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model, CancellationToken ct)
    {
        if (id != model.Id)
            return BadRequest();
        if (string.IsNullOrWhiteSpace(model.Name))
            ModelState.AddModelError(nameof(model.Name), "Kategori adı zorunludur.");
        if (!ModelState.IsValid)
            return View(model);

        _db.Update(model);
        await _db.SaveChangesAsync(ct);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await _db.Categories.FindAsync([id], ct);
        if (entity != null)
        {
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
        return RedirectToAction(nameof(Index));
    }
}
