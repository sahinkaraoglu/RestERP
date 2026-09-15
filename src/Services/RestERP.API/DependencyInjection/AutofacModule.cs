using Autofac;
using RestERP.Application.Services;
using RestERP.Application.Services.Abstract;
using RestERP.Application.Services.Concrete;
using RestERP.Core.Interfaces.Repositories;
using RestERP.Infrastructure.Repositories;

namespace RestERP.API.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Entity'ye özel repository'ler (UnitOfWork kaldırıldı; DbContext zaten
            // Scoped/InstancePerLifetimeScope olduğu için Unit of Work görevini görür)
            builder.RegisterType<FoodRepository>().As<IFoodRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FoodCategoryRepository>().As<IFoodCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ImageRepository>().As<IImageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderItemRepository>().As<IOrderItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TableRepository>().As<ITableRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReservationRepository>().As<IReservationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LogRepository>().As<ILogRepository>().InstancePerLifetimeScope();

            // Servisler (Transient => InstancePerDependency)
            builder.RegisterType<TableService>().As<ITableService>().InstancePerDependency();
            builder.RegisterType<FoodService>().As<IFoodService>().InstancePerDependency();
            builder.RegisterType<FoodCategoryService>().As<IFoodCategoryService>().InstancePerDependency();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerDependency();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterType<ReservationService>().As<IReservationService>().InstancePerDependency();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerDependency();
        }
    }
}


