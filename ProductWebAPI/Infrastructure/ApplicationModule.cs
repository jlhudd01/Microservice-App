using System.Reflection;
using Autofac;
using ProductWebAPI.Commands;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Infrastructure
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();

            // builder.RegisterAssemblyTypes(typeof(CreateProductCommandHandler).GetTypeInfo().Assembly)
            //     .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }        
    }
}