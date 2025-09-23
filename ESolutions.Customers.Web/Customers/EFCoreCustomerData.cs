using ESolutions.Customers.Web.Customers;
using Microsoft.EntityFrameworkCore;


public class EFCoreCustomerData(AppDbContext dbContext) : ICustomerData
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task Add(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteById(Guid id)
    {
        var customerToDelete = await _dbContext.Customers.FindAsync(id);
        if (customerToDelete is null) return;
        _dbContext.Remove(customerToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Customer?> GetById(Guid customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        return customer;
    }

    public async Task<IEnumerable<Customer>> List()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task Update(Guid id, Customer updatedCustomer)
    {
        await _dbContext.SaveChangesAsync();
    }
}
