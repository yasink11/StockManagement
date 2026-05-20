using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Data.Entities;

namespace StockManagement.Web.Controllers;

public class CustomerController : Controller
{
    private readonly TestDbContext _db;

    public CustomerController(TestDbContext db) => _db = db;

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<object> Get(DataSourceLoadOptions loadOptions, CancellationToken ct)
    {
        var query = _db.Customers.AsNoTracking();
        return await DataSourceLoader.LoadAsync(query, loadOptions, ct);
    }

    public IActionResult Create() => View(new Customer());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.Customertitle))
            ModelState.AddModelError(nameof(model.Customertitle), "Müşteri ünvanı zorunludur.");
        if (!ModelState.IsValid)
            return View(model);

        _db.Customers.Add(model);
        await _db.SaveChangesAsync(ct);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await _db.Customers.FindAsync([id], ct);
        if (entity == null)
            return NotFound();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer model, CancellationToken ct)
    {
        if (id != model.Id)
            return BadRequest();
        if (string.IsNullOrWhiteSpace(model.Customertitle))
            ModelState.AddModelError(nameof(model.Customertitle), "Müşteri ünvanı zorunludur.");
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
        var entity = await _db.Customers.FindAsync([id], ct);
        if (entity != null)
        {
            _db.Customers.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
        return RedirectToAction(nameof(Index));
    }
}
