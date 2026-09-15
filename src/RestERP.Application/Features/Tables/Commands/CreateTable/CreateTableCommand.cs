using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Tables.Commands.CreateTable
{
    public record CreateTableCommand(Table Table) : IRequest<Table>;

    public class CreateTableCommandHandler : IRequestHandler<CreateTableCommand, Table>
    {
        private readonly ITableService _tableService;

        public CreateTableCommandHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task<Table> Handle(CreateTableCommand request, CancellationToken cancellationToken)
            => _tableService.CreateTableAsync(request.Table);
    }
}
