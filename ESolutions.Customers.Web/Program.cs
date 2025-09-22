using ESolutions.Customers.Web.Customers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => 
config.SwaggerDoc("v1", new OpenApiInfo()
{
    Title = "(title)Esolutions Customers API",
    Description = "(description)API for managing customers",
    Version = "(version)1.0"
}));
builder.Services.AddSingleton<CustomerData>();

var mailSettings = builder.Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>();
builder.Services.AddSingleton(mailSettings);  


builder.Services.AddTransient<IEmailMessageFactory, EmailMessageFactory>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IEmailSenderService, ConsoleOnlyEmailSenderService>();
}
else
{
    builder.Services.AddTransient<IEmailSenderService, MailKitEmailSenderService>();

}

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
