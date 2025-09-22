using ESolutions.Customers.Web.Customers;

public class CustomerData
{
    private static List<Customer> _customers = new();

    public CustomerData()
    {
        string testEmail = "test@test.com";

        _customers.AddRange(new[]
        {
            new Customer(Guid.Parse("25c9868f-a724-4257-9758-09e372116936"),
                "Acme Inc.", testEmail, new()),
            new Customer(Guid.NewGuid(), "Contoso Ltd.", testEmail, new()),
            new Customer(Guid.NewGuid(), "Fabrikam Corp.", testEmail, new())
        });
        _customers[0].Projects.AddRange(new[]
        {
            new Project(Guid.NewGuid(), "Project 1", Guid.NewGuid()),
            new Project(Guid.NewGuid(), "Project 2", Guid.NewGuid())
        });
        _customers[1].Projects.AddRange(new[]
        {
            new Project(Guid.NewGuid(), "Project 3", Guid.NewGuid()),
            new Project(Guid.NewGuid(), "Project 4", Guid.NewGuid())
        });
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