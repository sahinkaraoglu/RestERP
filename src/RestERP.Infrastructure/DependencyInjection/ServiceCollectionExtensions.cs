using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestERP.Core.Interfaces.Logging;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Context;
using RestERP.Infrastructure.Logging;
using RestERP.Infrastructure.Repositories;

namespace RestERP.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestERPRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            return services;
        }

        public static IServiceCollection AddRestERPLogging(
            this IServiceCollection services,
            string loggingConnectionString)
        {
            services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(loggingConnectionString));

            services.AddScoped<IRequestLogWriter, RequestLogWriter>();

            return services;
        }
    }
}
