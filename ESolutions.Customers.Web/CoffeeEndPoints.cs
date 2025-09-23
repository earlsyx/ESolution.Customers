public static class CoffeeEndpoints
{
    public static void MapCoffeeEndpoints(this WebApplication app)
    {
        app.MapGet("/brewcoffee", (HttpResponse response) =>
        {
            response.StatusCode = 418;
            response.ContentType = "text/plain";
            return response.WriteAsync("I'm a teapot - I cannot brew coffee");
        })
            .WithName("BrewCoffee")
            .WithOpenApi();
    }
}