using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Commands.CreateFood
{
    public record CreateFoodCommand(Food Food) : IRequest<Food>;

    public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, Food>
    {
        private readonly IFoodService _foodService;

        public CreateFoodCommandHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<Food> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
            => _foodService.CreateFoodAsync(request.Food);
    }
}
