using MailKit.Net.Smtp;
using MimeKit;

namespace ESolutions.Customers.Web.Customers;

internal class CustomerEmailService
{
    private readonly IEmailMessageFactory _emailMessageFactory;
    private readonly IEmailSenderService _emailSenderService;

    public CustomerEmailService(IEmailMessageFactory emailMessageFactory,
        IEmailSenderService emailSenderService)
    {
        this._emailMessageFactory = emailMessageFactory;
        this._emailSenderService = emailSenderService;
    }

    internal async Task SendWelcomeEmail(Customer newCustomer)
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
public class MailKitEmailSenderService : IEmailSenderService
{
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        using (SmtpClient client = new SmtpClient()) // use local host and a test server
        {
            client.Connect("localhost", 25, false);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            await client.SendAsync(message);
            Console.WriteLine("Email Sent!");

            client.Disconnect(true);
        }
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

