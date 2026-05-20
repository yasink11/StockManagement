using StockManagement.DAL;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class ProductService
{
    private readonly ProductRepository _repo;

    public ProductService(ProductRepository repo) => _repo = repo;

    public List<Product> GetAll() => _repo.GetAll();
    public Product? GetById(int id) => _repo.GetById(id);

    public void Save(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new InvalidOperationException("Ürün adı zorunludur.");

        if (product.Id == 0)
            _repo.Insert(product);
        else
            _repo.Update(product);
    }

    public void Delete(int id) => _repo.Delete(id);
}
