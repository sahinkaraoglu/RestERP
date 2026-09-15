using MediatR;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Features.Foods.Queries.GetFoodImages
{
    public record GetFoodImagesQuery : IRequest<IEnumerable<Image>>;

    public class GetFoodImagesQueryHandler : IRequestHandler<GetFoodImagesQuery, IEnumerable<Image>>
    {
        private readonly IFoodService _foodService;

        public GetFoodImagesQueryHandler(IFoodService foodService)
        {
            _foodService = foodService;
        }

        public Task<IEnumerable<Image>> Handle(GetFoodImagesQuery request, CancellationToken cancellationToken)
            => _foodService.GetAllFoodImagesAsync();
    }
}
