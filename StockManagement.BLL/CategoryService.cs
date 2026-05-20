using StockManagement.DAL;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class CategoryService
{
    private readonly CategoryRepository _repo;

    public CategoryService(CategoryRepository repo) => _repo = repo;

    public List<Category> GetAll() => _repo.GetAll();
    public Category? GetById(int id) => _repo.GetById(id);
    public void Save(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new InvalidOperationException("Kategori adı zorunludur.");

        if (category.Id == 0)
            _repo.Insert(category);
        else
            _repo.Update(category);
    }

    public void Delete(int id) => _repo.Delete(id);
}
