using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RestERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}
