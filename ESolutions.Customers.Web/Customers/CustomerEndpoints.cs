using System.ComponentModel.DataAnnotations;

namespace ESolutions.Customers.Web.Customers;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var _customerGroup = app.MapGroup("/customers")
            .WithOpenApi()
            .WithTags("Customers");

        var _customerGroupWithValidation = _customerGroup.MapGroup("/")
            .WithParameterValidation();

        _customerGroup.MapGet("/", async (ICustomerData data) =>
        {
            var customers = await data.List();
            return TypedResults.Ok(customers);
        })
            .WithDescription("(description)List all customers.")
            .WithSummary("(summary)List Customers")
        .WithName("ListCustomers");

        _customerGroup.MapGet("/{customerId:guid}",
            async (Guid customerId, ICustomerData data) =>
            await data.GetById(customerId) is Customer customer
                ? TypedResults.Ok(customer)
                : Results.NotFound())
        .WithName("GetCustomerById");

        _customerGroupWithValidation.MapPost("/",
            async (CreateCustomerRequest request,
                    ICustomerData data,
                    ICustomerEmailService customerEmailService) =>
            {
                var newCustomer = new Customer(Guid.NewGuid(), request.CompanyName,
                    request.EmailAddress, new());
                await data.Add(newCustomer);

                await customerEmailService.SendWelcomeEmail(newCustomer);

                return Results.Created($"/customers/{newCustomer.Id}", newCustomer);
            })
            .Produces<Customer>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithName("AddCustomer");

        _customerGroupWithValidation.MapPut("/{id:guid}",
               async (Guid id, UpdateCustomerRequest request, ICustomerData data) =>
               {
                   var existingCustomer = await data.GetById(id);
                   if (existingCustomer is null)
                   {
                       return Results.NotFound();
                   }

                   var updatedCustomer = existingCustomer with
                   {
                       CompanyName = request.CompanyName,
                       Projects = request.Projects
                   };
                   await data.Update(id, updatedCustomer);
                   return Results.Ok(updatedCustomer);
               })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("UpdateCustomer");

        _customerGroup.MapDelete("/{id:guid}",
            [EndpointDescription("(description)Delete a customer.")]
        async (Guid id, ICustomerData data) =>
            {
                var customer = await data.GetById(id);
                if (customer is null)
                {
                    return Results.NotFound();
                }
                await data.DeleteById(id);

                return Results.NoContent();
            })
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("DeleteCustomer");
    }
}

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
                new[] { nameof(CompanyName) });
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
                new[] { nameof(CompanyName) });
        }
    }
}
