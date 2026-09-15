using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Tables.Queries.GetTableById
{
    public record GetTableByIdQuery(int Id) : IRequest<Table>;

    public class GetTableByIdQueryHandler : IRequestHandler<GetTableByIdQuery, Table>
    {
        private readonly ITableService _tableService;

        public GetTableByIdQueryHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task<Table> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
            => _tableService.GetTableByIdAsync(request.Id);
    }
}
