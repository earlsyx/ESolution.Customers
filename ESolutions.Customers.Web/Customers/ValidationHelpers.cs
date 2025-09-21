using ESolutions.Customers.Web.Customers;
//public readonly record struct PutRequest : IValidatableObject
//{
//    [FromRoute(Name = "Id")]
//    [Required]
//    public Guid id { get; init; }
//    [Required]
//    public Customer Customer { get; init; }
//    public CustomerData Data { get; init; }

//    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//    {
//        if (Customer.CompanyName  == "NimblePros")
//        {
//            yield return new ValidationResult("Company Name cannot be NimblePros.",
//                new[] { nameof(Customer.CompanyName) });        
//        }
//    }
//}

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