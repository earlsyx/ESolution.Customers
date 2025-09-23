using Microsoft.EntityFrameworkCore;

namespace ESolutions.Customers.Web.Customers;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
}
