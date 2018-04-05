using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderWebAPI.Contexts;
using OrderWebAPI.Infrastructure;
using OrderWebAPI.Models;
using OrderWebAPI.IntegrationEvents;
using OrderWebAPI.IntegrationEvents.EventHandlers;
using OrderWebAPI.IntegrationEvents.Events;
using OrderWebAPI.RabbitMQ;
using RabbitMQ.Client;

namespace OrderWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite()
                .AddDbContext<OrderContext>(opt => opt.UseSqlite("DataSource=database.db"));
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddTransient<IOrdersIntegrationEventService, OrdersIntegrationEventService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory();
                factory.HostName = "localhost";
                factory.UserName = "users";
                factory.Password = "users";

                return new DefaultRabbitMQPersistentConnection(factory, 5);
            });

            RegisterEventBus(services);

            services.AddOptions();

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var context = app.ApplicationServices.GetService<OrderContext>();
            SeedDatabase(context);

            app.UseCors("CorsPolicy");
            app.UseMvc();
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<ProductUpdateIntegrationEvent, IIntegrationEventHandler<ProductUpdateIntegrationEvent>>();
            eventBus.Subscribe<ProductDeleteIntegrationEvent, IIntegrationEventHandler<ProductDeleteIntegrationEvent>>();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifitimeScope = sp.GetRequiredService<ILifetimeScope>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, iLifitimeScope, eventBusSubscriptionsManager, "Products", 5);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        private static void SeedDatabase(OrderContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [Orders]");
            context.Database.ExecuteSqlCommand("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Orders'");
            context.Database.ExecuteSqlCommand("DELETE FROM [OrderItems]");
            context.Database.ExecuteSqlCommand("DELETE FROM SQLITE_SEQUENCE WHERE NAME='OrderItems'");
            context.SaveChanges();

            var orderItem1 = new OrderItem(
                    1,
                    "Product 1",
                    1.03m
                );
            var orderItem2 = new OrderItem(
                    2,
                    "Product 2",
                    2.03m
            );
            var orderItem3 = new OrderItem(
                    3,
                    "Product 3",
                    3.03m
            );
            var orderItem4 = new OrderItem(
                    1,
                    "Product 1",
                    1.03m
                );
            var order1 = new Order(new List<OrderItem>()
                        {
                            orderItem1,
                            orderItem2
                        });
            var order2 = new Order(new List<OrderItem>()
                        {
                            orderItem3,
                            orderItem4
                        });
            var orders = new List<Order>()
                {
                    order1,
                    order2
                };

            context.Orders.AddRange(orders);

            context.SaveChanges();
        }
    }
}
