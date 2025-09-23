using MimeKit;
using MailKit.Net.Smtp;

namespace ESolutions.Customers.Web.Customers;

public record MailSettings(int Port, string Server);
public class MailKitEmailSenderService(IConfiguration config,
    MailSettings mailSettings) : IEmailSenderService
{
    private readonly IConfiguration _config = config;
    private readonly MailSettings _mailSettings = mailSettings;

    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        int port = int.Parse(_config["MailSettings:Port"]);
        using (SmtpClient client = new SmtpClient()) // use localhost and a test server
        {
            client.Connect(_mailSettings.Server, _mailSettings.Port, false);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            await client.SendAsync(message);
            Console.WriteLine("Email sent!");

            client.Disconnect(true);
        }
    }

}
