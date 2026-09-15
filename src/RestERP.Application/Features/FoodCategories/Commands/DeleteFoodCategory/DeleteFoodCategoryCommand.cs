using MediatR;
using RestERP.Application.Services.Abstract;

namespace RestERP.Application.Features.FoodCategories.Commands.DeleteFoodCategory
{
    public record DeleteFoodCategoryCommand(int Id) : IRequest;

    public class DeleteFoodCategoryCommandHandler : IRequestHandler<DeleteFoodCategoryCommand>
    {
        private readonly IFoodCategoryService _foodCategoryService;

        public DeleteFoodCategoryCommandHandler(IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
        }

        public Task Handle(DeleteFoodCategoryCommand request, CancellationToken cancellationToken)
            => _foodCategoryService.DeleteCategoryAsync(request.Id);
    }
}
