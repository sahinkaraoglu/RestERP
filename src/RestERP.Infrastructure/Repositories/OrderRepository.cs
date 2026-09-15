using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(RestERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}
