namespace ESolutions.Customers.Web.Customers;

public record Project
{
    public Project(Guid Id, string ProjectName, Guid CustomerId)
    {
        this.Id = Id;
        this.ProjectName = ProjectName;
        this.CustomerId = CustomerId;
    }

    private Project() // EF Core constructor
    {
    }

    public Guid Id { get; private set; }
    public string ProjectName { get; private set; }
    public Guid CustomerId { get; private set; }
}
