using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    return TypedResults.Ok(customers);
})
.WithName("ListCustomers")
.WithOpenApi();

app.MapGet("/customers/{id:guid}", async (Guid id, CustomerData data) =>

    await data.GetIdByAsync(id) is Customer customer
        ? TypedResults.Ok(customer)
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
        //if (String.IsNullOrEmpty(customer.CompanyName))
        //{
        //    return Results.ValidationProblem(new Dictionary<string, string[]>
        //{
        //    {"CompanyName", new[] {"Company name is required"} }
        //});
        //    //    return Results.BadRequest(new { CompanyName = "Company name is required" });
        //}
        var newCustomer = customer with { Id = Guid.NewGuid(), Projects = new() };
        await data.AddAsync(newCustomer);
        return Results.Created($"/customers/{newCustomer.Id}", newCustomer);
    })
    //.AddEndpointFilter(ValidationHelpers.ValidateAddCustomer)
    .AddEndpointFilter<ValidateCustomer>()
    
.WithName("AddCustomer")
.WithOpenApi();


app.MapPut("/customers/{id:guid}",
    async ([AsParameters] PutRequest request) =>
{
    //if (String.IsNullOrEmpty(customer.CompanyName))
    //{
    //    return Results.ValidationProblem(new Dictionary<string, string[]>
    //{
    //    {"CompanyName", new[] {"Company name is required"} }
    //});
    //    //    return Results.BadRequest(new { CompanyName = "Company name is required" });
    //}
    var existingCustomer = await request.Data.GetIdByAsync(request.id);
    if (existingCustomer is null) return Results.NotFound();

    var updatedCustomer = existingCustomer with
    {
        CompanyName = request.Customer.CompanyName,
        Projects = request.Customer.Projects ?? new List<Project>()
    };
    await request.Data.UpdateAsync(updatedCustomer);
    return Results.Ok(updatedCustomer);
})
    //.AddEndpointFilter(ValidationHelpers.ValidateUpdateCustomer)
    //.AddEndpointFilter<ValidateCustomer>()
    .WithParameterValidation()
.WithName("UpdateCustomer")
.WithOpenApi();

app.MapDelete("/customers/{id:guid}", async (Guid id,
    CustomerData data) =>
{

    if (await data.GetIdByAsync(id) is null) return Results.NotFound();

    await data.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteCustomer")
.WithOpenApi();

app.MapGet("/brewcofee", (HttpResponse response) =>
{
    response.StatusCode = 418;
    response.ContentType = "text/plain";
    return response.WriteAsync("I'm a teapot - I cannot brew coffee");
})
    .WithName("BrewCoffee")
    .WithOpenApi();

app.Run();

public record Customer(Guid Id, [MinLength(10)][Required]string CompanyName, List<Project> Projects);
public record Project(Guid Id, string ProjectName, Guid CustomerId);

public readonly record struct PutRequest : IValidatableObject
{
    [FromRoute(Name = "Id")]
    [Required]
    public Guid id { get; init; }
    [Required]
    public Customer Customer { get; init; }
    public CustomerData Data { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Customer.CompanyName  == "NimblePros")
        {
            yield return new ValidationResult("Company Name cannot be NimblePros.",
                new[] { nameof(Customer.CompanyName) });        
        }
    }
}

public class ValidationHelpers
{
    internal static async ValueTask<object?> ValidateAddCustomer(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        //Check if invalid

        var customer = context.GetArgument<Customer>(0);

        if (customer is not null && String.IsNullOrEmpty(customer.CompanyName)) 
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"CompanyName" , new[] { "Company name is required"} }
                });
        }

        return await next(context);
    }
    internal static async ValueTask<object?> ValidateUpdateCustomer(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        //Check if invalid

        var customer = context.GetArgument<Customer>(1);

        if (customer is not null && String.IsNullOrEmpty(customer.CompanyName))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"CompanyName" , new[] { "Company name is required"} }
                });
        }

        return await next(context);
    }
}

public class ValidateCustomer : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var customer = context.Arguments.FirstOrDefault(a => a is Customer) as Customer;

        if (customer is not null && String.IsNullOrEmpty(customer.CompanyName))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"CompanyName" , new[] { "Company name is required"} }
                });
        }

        return await next(context);
    }
}

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