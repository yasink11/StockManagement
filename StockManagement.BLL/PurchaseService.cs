using StockManagement.DAL;
using StockManagement.Entities.Dtos;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class PurchaseService
{
    private readonly PurchaseRepository _repo;

    public PurchaseService(PurchaseRepository repo) => _repo = repo;

    public List<PurchaseListItem> GetAll() => _repo.GetAllFromSp();

    public void Create(Purchase purchase)
    {
        if (purchase.ProductId is null or <= 0)
            throw new InvalidOperationException("Ürün seçilmelidir.");
        if (purchase.CustomerId is null or <= 0)
            throw new InvalidOperationException("Müşteri seçilmelidir.");
        if (purchase.Quantity is null or <= 0)
            throw new InvalidOperationException("Miktar 0'dan büyük olmalıdır.");
        if (purchase.Price is null or < 0)
            throw new InvalidOperationException("Birim fiyat geçersiz.");

        purchase.Date ??= DateTime.Now;
        purchase.Amount = purchase.Quantity * purchase.Price;
        _repo.Insert(purchase);
    }
}
