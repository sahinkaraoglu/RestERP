using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Tables.Commands.UpdateTable
{
    public record UpdateTableCommand(Table Table) : IRequest;

    public class UpdateTableCommandHandler : IRequestHandler<UpdateTableCommand>
    {
        private readonly ITableService _tableService;

        public UpdateTableCommandHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task Handle(UpdateTableCommand request, CancellationToken cancellationToken)
            => _tableService.UpdateTableAsync(request.Table);
    }
}
