using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Tables.Commands.SetTableOccupiedStatus
{
    public record SetTableOccupiedStatusCommand(int Id, bool IsOccupied) : IRequest<bool>;

    public class SetTableOccupiedStatusCommandHandler : IRequestHandler<SetTableOccupiedStatusCommand, bool>
    {
        private readonly ITableService _tableService;

        public SetTableOccupiedStatusCommandHandler(ITableService tableService)
        {
            _tableService = tableService;
        }

        public Task<bool> Handle(SetTableOccupiedStatusCommand request, CancellationToken cancellationToken)
            => _tableService.SetTableOccupiedStatusAsync(request.Id, request.IsOccupied);
    }
}
