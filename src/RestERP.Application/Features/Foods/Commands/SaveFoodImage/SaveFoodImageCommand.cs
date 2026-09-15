using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.Foods.Commands.SaveFoodImage
{
    public record SaveFoodImageCommand(int FoodId, string Path) : IRequest;

    public class SaveFoodImageCommandHandler : IRequestHandler<SaveFoodImageCommand>
    {
        private readonly IFoodService _foodService;

        public SaveFoodImageCommandHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task Handle(SaveFoodImageCommand request, CancellationToken cancellationToken)
            => _foodService.SaveFoodImageAsync(request.FoodId, request.Path);
    }
}
