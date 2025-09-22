using ESolutions.Customers.Web.Customers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CustomerData>();
builder.Services.AddTransient<IEmailMessageFactory, EmailMessageFactory>();
builder.Services.AddTransient<IEmailSenderService, ConsoleOnlyEmailSenderService>();
builder.Services.AddTransient<ICustomerEmailService, CustomerEmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.MapCoffeeEndPoints();

app.Run();
