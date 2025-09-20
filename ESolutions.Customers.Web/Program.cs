var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CustomerData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/customers", async (CustomerData data) =>
{
    var customers = await data.ListAsync();
    return customers;
})
.WithName("ListCustomers")
.WithOpenApi();

app.MapGet("/customers/{id:guid}", async (Guid id, CustomerData data) =>

    await data.GetIdByAsync(id) is Customer customer
        ? Results.Ok(customer)
        : Results.NotFound()
    //Customer? customer = await data.GetByIdAsync(id);
    //if (customer is null)
    //{
    //    return Results.NotFound();
    //}
    //return customer;
)
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", async (Customer customer, CustomerData data) =>
{
    var newCustomer = customer with { Id = Guid.NewGuid(), Projects = new() };
    await data.AddAsync(newCustomer);
    return Results.Created($"/customers/{newCustomer.Id}", newCustomer);
})
.WithName("AddCustomer")
.WithOpenApi();


app.MapPut("/customers/{id:guid}", async (Guid id,
    Customer customer, CustomerData data) =>
{
    var existingCustomer = await data.GetIdByAsync(id);
    if (existingCustomer is null) return Results.NotFound();

    var updatedCustomer = existingCustomer with
    {
        CompanyName = customer.CompanyName,
        Projects = customer.Projects
    };
    await data.UpdateAsync(customer);
    return Results.Ok(customer);
})
.WithName("UpdateCustomer")
.WithOpenApi();

app.Run();

public record Customer(Guid Id, string CompanyName, List<Project> Projects);
public record Project(Guid Id, string ProjectName, Guid CustomerId);

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

    public Task UpdateAsync(Customer customer)
    {
        if (_customers.Any(c => c.Id == customer.Id))
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);
            _customers[index] = customer;
        }

        return Task.CompletedTask;
    }
}  