using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using OrderWebAPI.Commands;
using OrderWebAPI.Commands.Validators;

namespace OrderWebAPI.Infrastructure
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(RemoveOrderItemFromOrderCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(RemoveOrderItemFromOrderCommandValidator).GetTypeInfo().Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces();

            //need this for mediator.send (command handler)
            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });
            //need this for mediator.publish (domain event handler)
            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t =>
                {
                    return (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                };
            });

            builder.RegisterGeneric(typeof(ValidatorFactory<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}