using ESolutions.Customers.Web.Customers;

namespace ESolutions.Customers.Web.Customers;

public record Customer
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string EmailAddress { get; set; }
    public List<Project> Projects { get; set; } = new();

    public Customer(Guid Id,
                    string CompanyName,
                    string EmailAddress,
                    List<Project> Projects)
    {
        this.Id = Id;
        this.CompanyName = CompanyName;
        this.EmailAddress = EmailAddress;
        this.Projects = Projects;
    }

    private Customer() // EF Core constructor
    {
    }
}