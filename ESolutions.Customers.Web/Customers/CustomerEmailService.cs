namespace ESolutions.Customers.Web.Customers;


public class CustomerEmailService : ICustomerEmailService
{
    private readonly IEmailMessageFactory _emailMessageFactory;
    private readonly IEmailSenderService _emailSenderService;

    public CustomerEmailService(IEmailMessageFactory emailMessageFactory,
        IEmailSenderService emailSenderService)
    {
        this._emailMessageFactory = emailMessageFactory;
        this._emailSenderService = emailSenderService;
    }

    public async Task SendWelcomeEmail(Customer newCustomer)
    {
        string from = "donotreply@nimblepros.com";
        string to = newCustomer.EmailAddress;
        string subject = "Welcome!";

        string body = _emailMessageFactory.GenerateWelcomeEmailBody(newCustomer);

        Console.WriteLine("Attempting to send email to {to} from {from} with subject {subject}...", to, from, subject);

        await _emailSenderService.SendEmailAsync(from, to, subject, body);
    }


}

public class ConsoleOnlyEmailSenderService : IEmailSenderService
{
    public Task SendEmailAsync(string from, string to, string subject, string body)
    {
        Console.WriteLine($"Sending email from {from} to {to} with subject {subject} and body {body}");
        return Task.CompletedTask;
    }

}

public class EmailMessageFactory : IEmailMessageFactory
{
    public string GenerateWelcomeEmailBody(Customer newCustomer)
    {
        string template = "Welcome to NimblePros, {{CompanyName}}!";
        string body = template.Replace("{{CompanyName}}", newCustomer.CompanyName);
        return body;
    }
}

