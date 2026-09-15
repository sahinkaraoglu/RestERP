using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(RestERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}
