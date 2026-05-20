using StockManagement.DAL;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class StockService
{
    private readonly StockRepository _repo;

    public StockService(StockRepository repo) => _repo = repo;

    public List<StockEntry> GetAll() => _repo.GetAll();

    public void Add(StockEntry entry)
    {
        if (entry.ProductId is null or <= 0)
            throw new InvalidOperationException("Ürün seçilmelidir.");
        if (entry.Quantity is null or <= 0)
            throw new InvalidOperationException("Miktar 0'dan büyük olmalıdır.");

        entry.Date ??= DateTime.Now;
        _repo.Insert(entry);
    }

    public double GetAvailableQuantity(int productId) => _repo.GetAvailableQuantity(productId);
}
