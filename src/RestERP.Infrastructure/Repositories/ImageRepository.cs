using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(RestERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}
