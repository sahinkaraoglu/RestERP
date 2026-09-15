using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Commands.UpdateFood
{
    public record UpdateFoodCommand(Food Food) : IRequest;

    public class UpdateFoodCommandHandler : IRequestHandler<UpdateFoodCommand>
    {
        private readonly IFoodService _foodService;

        public UpdateFoodCommandHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
            => _foodService.UpdateFoodAsync(request.Food);
    }
}
