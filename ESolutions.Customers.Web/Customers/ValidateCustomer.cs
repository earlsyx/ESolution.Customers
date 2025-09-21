using ESolutions.Customers.Web.Customers;

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