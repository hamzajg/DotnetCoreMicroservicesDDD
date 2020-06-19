using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entites = Customer.Domain.Entites;
namespace Customer.App.Domain
{
    public interface CustomerRepository
    {
         Task<IEnumerable<Entites.Customer>> GetAllAsync();

         Task<Entites.Customer> SaveAsync(Entites.Customer customer);

         Task<Entites.Customer> UpdateAsync(Guid customerId, Entites.Customer customer);
    }
}