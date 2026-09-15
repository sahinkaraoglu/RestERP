using Microsoft.Extensions.DependencyInjection;
using RestERP.Application.Services;
using RestERP.Application.Services.Abstract;
using RestERP.Application.Services.Concrete;

namespace RestERP.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestERPApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
