using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customer.App.Domain;

namespace Customer.App.Repositories
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        public Task<IEnumerable<Domain.Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Customer> SaveAsync(Domain.Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Customer> UpdateAsync(Guid customerId, Domain.Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}