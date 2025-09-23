using ESolutions.Customers.Web.Customers;

namespace NimblePros.Customers.Web.Customers;

public class CustomerData : ICustomerData
{
    private static List<Customer> _customers = new();

    static CustomerData()
    {
        string testEmail = "test@test.com";

        _customers.AddRange(new[]
        {
            new Customer(Guid.Parse("456f88c4-9727-4cb2-b19a-fe34836dc2d4"),
                "Acme Inc.", testEmail, new()),
            new Customer(Guid.NewGuid(), testEmail, "Contoso Ltd.", new()),
            new Customer(Guid.NewGuid(), testEmail, "Fabrikam Corp.", new())
        });
        _customers[0].Projects.AddRange(new[]
        {
            new Project(Guid.NewGuid(), "Project 1", _customers[0].Id),
            new Project(Guid.NewGuid(), "Project 2", _customers[0].Id)
        });
        _customers[1].Projects.AddRange(new[]
        {
            new Project(Guid.NewGuid(), "Project 3", _customers[1].Id),
            new Project(Guid.NewGuid(), "Project 4", _customers[1].Id)
        });
    }

    public Task<IEnumerable<Customer>> List()
    {
        return Task.FromResult(_customers.ToArray().AsEnumerable());
    }

    public Task<Customer?> GetById(Guid customerId)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == customerId);
        return Task.FromResult(customer);
    }

    public Task Add(Customer customer)
    {
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task Update(Guid id, Customer updatedCustomer)
    {
        if (_customers.Any(c => c.Id == id))
        {
            var index = _customers.FindIndex(c => c.Id == id);
            _customers[index] = updatedCustomer;
        }
        return Task.CompletedTask;
    }

    public Task DeleteById(Guid id)
    {
        if (_customers.Any(c => c.Id == id))
        {
            var index = _customers.FindIndex(c => c.Id == id);
            _customers.RemoveAt(index);
        }
        return Task.CompletedTask;
    }
}