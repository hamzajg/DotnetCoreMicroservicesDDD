using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer.App.Domain;
using Entites = Customer.Domain.Entites;

namespace Customer.App.Repositories
{
    public class InMemoryCustomerRepository : CustomerRepository
    {
        public Task<IEnumerable<Entites.Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Entites.Customer> SaveAsync(Entites.Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Entites.Customer> UpdateAsync(Guid customerId, Entites.Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}