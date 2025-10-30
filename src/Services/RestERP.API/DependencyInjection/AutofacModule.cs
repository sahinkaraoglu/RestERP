using Autofac;
using RestERP.Application.Services;
using RestERP.Application.Services.Abstract;
using RestERP.Application.Services.Concrete;
using RestERP.Core.Interfaces;
using RestERP.Infrastructure.Repositories;

namespace RestERP.API.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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


