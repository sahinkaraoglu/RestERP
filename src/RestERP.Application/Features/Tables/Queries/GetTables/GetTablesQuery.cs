using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Tables.Queries.GetTables
{
    public record GetTablesQuery : IRequest<IEnumerable<Table>>;

    public class GetTablesQueryHandler : IRequestHandler<GetTablesQuery, IEnumerable<Table>>
    {
        private readonly ITableService _tableService;

        public GetTablesQueryHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task<IEnumerable<Table>> Handle(GetTablesQuery request, CancellationToken cancellationToken)
            => _tableService.GetAllTablesAsync();
    }
}
