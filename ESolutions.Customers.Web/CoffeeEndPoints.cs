public static class CoffeeEndPoints
{
    public static void MapCoffeeEndPoints(this WebApplication app)
    {
        app.MapGet("/brewcofee", (HttpResponse response) =>
        {
            response.StatusCode = 418;
            response.ContentType = "text/plain";
            return response.WriteAsync("I'm a teapot - I cannot brew coffee");
        })
      .WithName("BrewCoffee")
      .WithOpenApi();
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