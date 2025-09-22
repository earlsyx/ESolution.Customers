using MailKit.Net.Smtp;
using MimeKit;

namespace ESolutions.Customers.Web.Customers;

public class MailKitEmailSenderService(IConfiguration config) : IEmailSenderService
{
    private readonly IConfiguration _config = config;
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        int port = int.Parse(_config["MailSettings:Port"]); 
        using (SmtpClient client = new SmtpClient()) // use local host and a test server
        {
            client.Connect("localhost", port, false);
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

