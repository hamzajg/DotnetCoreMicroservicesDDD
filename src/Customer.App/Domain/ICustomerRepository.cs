using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Customer.App.Domain
{
    public interface ICustomerRepository
    {
         Task<IEnumerable<Customer>> GetAllAsync();

         Task<Customer> SaveAsync(Customer customer);

         Task<Customer> UpdateAsync(Guid customerId, Customer customer);
    }
}