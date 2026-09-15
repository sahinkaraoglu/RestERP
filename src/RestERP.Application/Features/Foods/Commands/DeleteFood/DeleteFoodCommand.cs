using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Foods.Commands.DeleteFood
{
    public record DeleteFoodCommand(int Id) : IRequest;

    public class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand>
    {
        private readonly IFoodService _foodService;

        public DeleteFoodCommandHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
            => _foodService.DeleteFoodAsync(request.Id);
    }
}
