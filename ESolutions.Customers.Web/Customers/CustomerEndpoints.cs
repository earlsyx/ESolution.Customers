using System.ComponentModel.DataAnnotations;

namespace ESolutions.Customers.Web.Customers;
public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var _customerGroup = app.MapGroup("/customers")
    .WithOpenApi();

        var _customerGroupWithValidation = _customerGroup.MapGroup("/")
            .WithParameterValidation();



        _customerGroup.MapGet("/", async (CustomerData data) =>
        {
            var customers = await data.ListAsync();
            return TypedResults.Ok(customers);
        })
        .WithName("ListCustomers")
        .WithOpenApi();

        _customerGroup.MapGet("/{id:guid}", async (Guid id, CustomerData data) =>

            await data.GetIdByAsync(id) is Customer customer
                ? TypedResults.Ok(customer)
                : Results.NotFound()
        )
        .WithName("GetCustomerById");


        _customerGroupWithValidation.MapPost("/",
            async (CreateCustomerRequest request, 
                    CustomerData data,
                    ICustomerEmailService customerEmailService) =>
            {
                var newCustomer = new Customer(Guid.NewGuid(), request.CompanyName, request.EmailAddress, new());
                await data.AddAsync(newCustomer);

              

                await customerEmailService.SendWelcomeEmail(newCustomer);
                return Results.Created($"/customers/{newCustomer.Id}", newCustomer);
            })
            .AddEndpointFilter<ValidateCustomer>()

        .WithName("AddCustomer")
        ;


        _customerGroupWithValidation.MapPut("/{id:guid}",
            async (Guid id, UpdateCustomerRequest request, CustomerData data) =>
            {
                var existingCustomer = await data.GetIdByAsync(id);
                if (existingCustomer is null) return Results.NotFound();

                var updatedCustomer = existingCustomer with
                {
                    CompanyName = request.CompanyName,
                    Projects = request.Projects ?? new List<Project>()
                };
                await data.UpdateAsync(id, updatedCustomer);
                return Results.Ok(updatedCustomer);
            })
        .WithName("UpdateCustomer")
        ;

        _customerGroup.MapDelete("/{id:guid}", async (Guid id,
            CustomerData data) =>
        {

            if (await data.GetIdByAsync(id) is null) return Results.NotFound();

            await data.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteCustomer");
        

    }

}

public record Customer(Guid Id, string CompanyName, string EmailAddress ,List<Project> Projects);
public record Project(Guid Id, string ProjectName, Guid CustomerId);

public readonly record struct UpdateCustomerRequest : IValidatableObject
{
    [Required]
    [MinLength(10)]
    public string CompanyName { get; init; }
    public List<Project> Projects { get; init; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        if (CompanyName == "NimblePros")
        {
            yield return new ValidationResult("Company Name cannot be NimblePros.",
                new[] { nameof(Customer.CompanyName) });
        }
    }
}



public readonly record struct CreateCustomerRequest : IValidatableObject
{
    [Required]
    [MinLength(10)]
    public string CompanyName { get; init; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; init; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        if (CompanyName == "NimblePros")
        {
            yield return new ValidationResult("Company Name cannot be NimblePros.",
                new[] { nameof(Customer.CompanyName) });
        }
    }
}