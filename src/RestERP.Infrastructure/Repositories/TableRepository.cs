using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(RestERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}
