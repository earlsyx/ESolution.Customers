namespace ESolutions.Customers.Web.Customers;

public interface IEmailMessageFactory
{
    string GenerateWelcomeEmailBody(Customer newCustomer);
}
