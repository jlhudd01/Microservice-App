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
using OrderWebAPI.ErrorHandling;
using System.Reflection;

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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<OrderContext>(options => 
                {
                     options.UseSqlServer(Configuration["ConnectionString"],
                     sqlServerOptionsAction: sqlOptions => 
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                    });
                 },
                ServiceLifetime.Scoped);

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.Configure<OrderSettings>(Configuration);

            services.AddTransient<IOrdersIntegrationEventService, OrdersIntegrationEventService>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory();
                factory.HostName = Configuration["EventBusConnection"];
                factory.UserName = Configuration["EventBusUser"];
                factory.Password = Configuration["EventBusPassword"];

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
    }
}
