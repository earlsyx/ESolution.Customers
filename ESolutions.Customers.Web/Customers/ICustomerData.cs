
namespace ESolutions.Customers.Web.Customers
{
    public interface ICustomerData
    {
        Task Add(Customer customer);
        Task DeleteById(Guid id);
        Task<Customer?> GetById(Guid customerId);
        Task<IEnumerable<Customer>> List();
        Task Update(Guid id, Customer updatedCustomer);
    }
}