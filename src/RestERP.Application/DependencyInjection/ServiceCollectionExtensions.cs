using Microsoft.Extensions.DependencyInjection;
using RestERP.Application.Features.Auth;
using RestERP.Application.Services.Abstract;
using RestERP.Application.Services.Concrete;

namespace RestERP.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestERPApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            services.AddScoped<AuthTokenService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
