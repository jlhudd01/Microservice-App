using System.Reflection;
using Autofac;
using OrderWebAPI.IntegrationEvents.EventHandlers;
using OrderWebAPI.Repositories;

namespace OrderWebAPI.Infrastructure
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ProductUpdateIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        } 
    }
}