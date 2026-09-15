using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.FoodCategories.Commands.UpdateFoodCategory
{
    public record UpdateFoodCategoryCommand(FoodCategory Category) : IRequest;

    public class UpdateFoodCategoryCommandHandler : IRequestHandler<UpdateFoodCategoryCommand>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public UpdateFoodCategoryCommandHandler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public Task Handle(UpdateFoodCategoryCommand request, CancellationToken cancellationToken)
            => _foodCategoryService.UpdateCategoryAsync(request.Category);
    }
}
