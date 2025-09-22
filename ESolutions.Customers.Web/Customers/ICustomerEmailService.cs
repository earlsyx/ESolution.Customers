using ESolutions.Customers.Web.Customers;

public interface ICustomerEmailService
{
    Task SendWelcomeEmail(Customer newCustomer);
}