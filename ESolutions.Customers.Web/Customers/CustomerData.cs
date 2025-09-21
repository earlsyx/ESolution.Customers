using ESolutions.Customers.Web.Customers;

public class CustomerData
{
    private readonly Guid _customer1Id = Guid.Parse("25c9868f-a724-4257-9758-09e372116936");
    private readonly Guid _customer2Id = Guid.Parse("965057ae-a00e-4025-afb3-0a19a61be870");
    private readonly List<Customer> _customers;

    public CustomerData()
    {
        _customers = new List<Customer>
        {
            new Customer(_customer1Id, "Acme", new List<Project>
            {
                new Project(Guid.NewGuid(), "Project 1", Guid.NewGuid()),
                new Project(Guid.NewGuid(), "Project 2", Guid.NewGuid())
            }),
            new Customer(_customer2Id, "Contoso", new List<Project>
            {
                new Project(Guid.NewGuid(), "Project 3", Guid.NewGuid()),
                new Project(Guid.NewGuid(), "Project 4", Guid.NewGuid())
            })
        };
    }
    public Task<List<Customer>> ListAsync()
    {
        return Task.FromResult(_customers);
    }

    public Task<Customer?> GetIdByAsync(Guid id)
    {
        return Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    }

    public Task AddAsync(Customer newCustomer)
    {
        _customers.Add(newCustomer);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Guid id, Customer customer)
    {
        if (_customers.Any(c => c.Id == customer.Id))
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);
            _customers[index] = customer;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (_customers.Any(c => c.Id == id))
        {
            var index = _customers.FindIndex(c => c.Id == id);
            _customers.RemoveAt(index);
        }

        return Task.CompletedTask;
    }


}



// MapGet ("/{id}", (int id) => ...)
// ModelBinding - Describing the process of asp.net core uses to take in different arguments that come in to an http request
// and bind them to whatever model type or types you have specified in your end point.
// Route
// Query string ? count =10&color=blue
// Headers
// Body (Json)
// Dependency Injected Services
// Custom 

// Bool TryParse (string input, out T result)

//HttpContext, HttpRequest, HttpResponse, CancellationToken, Claims Principal,Steranm PipeReader, IFormFile 