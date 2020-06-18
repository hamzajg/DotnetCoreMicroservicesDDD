using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Customer.App.Domain
{
    public interface ICustomerRepository
    {
         Task<IEnumerable<Customer.Domain.Entites.Customer>> GetAllAsync();

         Task<Customer.Domain.Entites.Customer> SaveAsync(Customer.Domain.Entites.Customer customer);

         Task<Customer.Domain.Entites.Customer> UpdateAsync(Guid customerId, Customer.Domain.Entites.Customer customer);
    }
}