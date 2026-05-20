using StockManagement.DAL;
using StockManagement.Entities.Models;

namespace StockManagement.BLL;

public class CustomerService
{
    private readonly CustomerRepository _repo;

    public CustomerService(CustomerRepository repo) => _repo = repo;

    public List<Customer> GetAll() => _repo.GetAll();
    public Customer? GetById(int id) => _repo.GetById(id);

    public void Save(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.CustomerTitle))
            throw new InvalidOperationException("Müşteri ünvanı zorunludur.");

        if (customer.Id == 0)
            _repo.Insert(customer);
        else
            _repo.Update(customer);
    }

    public void Delete(int id) => _repo.Delete(id);
}
