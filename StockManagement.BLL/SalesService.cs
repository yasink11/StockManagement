using StockManagement.DAL;
using StockManagement.Entities.Dtos;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class SalesService
{
    private readonly SalesRepository _salesRepo;
    private readonly ProductRepository _productRepo;
    private readonly StockRepository _stockRepo;

    public SalesService(SalesRepository salesRepo, ProductRepository productRepo, StockRepository stockRepo)
    {
        _salesRepo = salesRepo;
        _productRepo = productRepo;
        _stockRepo = stockRepo;
    }

    public List<SalesListItem> GetAll() => _salesRepo.GetAllFromSp();
    public List<SalesReportItem> GetDetailReport() => _salesRepo.GetSalesDetailReport();

    public void Create(Sale sale)
    {
        if (sale.ProductId is null or <= 0)
            throw new InvalidOperationException("Ürün seçilmelidir.");
        if (sale.CustomerId is null or <= 0)
            throw new InvalidOperationException("Müşteri seçilmelidir.");
        if (sale.Quantity is null or <= 0)
            throw new InvalidOperationException("Miktar 0'dan büyük olmalıdır.");

        var available = _stockRepo.GetAvailableQuantity(sale.ProductId.Value);
        if (sale.Quantity > available)
            throw new InvalidOperationException($"Yetersiz stok. Mevcut: {available}");

        var product = _productRepo.GetById(sale.ProductId.Value);
        sale.ListPrice ??= product?.SalesPrice ?? sale.SalesPrice;
        sale.SalesPrice ??= product?.SalesPrice;
        sale.DiscountRate ??= 0;

        var discountMultiplier = 1 - (sale.DiscountRate / 100.0);
        sale.Amount = sale.Quantity * sale.SalesPrice * discountMultiplier;
        sale.Date ??= DateTime.Now;

        _salesRepo.Insert(sale);
    }
}
