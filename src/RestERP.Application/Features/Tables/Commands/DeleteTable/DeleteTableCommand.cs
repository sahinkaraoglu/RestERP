using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Tables.Commands.DeleteTable
{
    public record DeleteTableCommand(int Id) : IRequest;

    public class DeleteTableCommandHandler : IRequestHandler<DeleteTableCommand>
    {
        private readonly ITableService _tableService;

        public DeleteTableCommandHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task Handle(DeleteTableCommand request, CancellationToken cancellationToken)
            => _tableService.DeleteTableAsync(request.Id);
    }
}
