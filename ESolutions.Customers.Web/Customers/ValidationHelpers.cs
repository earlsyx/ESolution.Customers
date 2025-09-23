namespace ESolutions.Customers.Web.Customers;
public class ValidationHelpers
{
    internal static async ValueTask<object?> ValidateAddCustomer(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // check if invalid
        var customer = context.GetArgument<Customer>(0);

        if (customer is not null && String.IsNullOrEmpty(customer.CompanyName))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "CompanyName", new[] { "Company name is required" } }
            });
        }

        return await next(context);
    }
    internal static async ValueTask<object?> ValidateUpdateCustomer(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // check if invalid
        var customer = context.GetArgument<Customer>(1);

        if (customer is not null && String.IsNullOrEmpty(customer.CompanyName))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "CompanyName", new[] { "Company name is required" } }
            });
        }

        return await next(context);
    }
}
