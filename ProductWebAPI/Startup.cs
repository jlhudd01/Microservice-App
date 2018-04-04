using System;
using System.Collections.Generic;
using System.Data.Common;
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
using ProductWebAPI.Contexts;
using ProductWebAPI.Infrastructure;
using ProductWebAPI.IntegrationEvents;
using ProductWebAPI.LogService;
using ProductWebAPI.Models;
using ProductWebAPI.RabbitMQ;
using RabbitMQ.Client;

namespace ProductWebAPI
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
                .AddDbContext<ProductContext>(opt => opt.UseSqlite("DataSource=database.db"), ServiceLifetime.Scoped);
                services.AddEntityFrameworkSqlite()
                .AddDbContext<IntegrationEventLogContext>(opt => opt.UseSqlite("DataSource=database.db"), ServiceLifetime.Scoped);
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<IProductsIntegrationEventService, ProductsIntegrationEventService>();

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

            var context = app.ApplicationServices.GetService<ProductContext>();
            var context2 = app.ApplicationServices.GetService<IntegrationEventLogContext>();
            SeedProductDatabase(context);
            DeleteEventLogDatabase(context2);

            app.UseCors("CorsPolicy");
            app.UseMvc();
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            //subscribe to integration events when needed
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

        private static void SeedProductDatabase(ProductContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [Products]");
            context.Database.ExecuteSqlCommand("DELETE FROM SQLITE_SEQUENCE WHERE NAME='Products'");
            context.SaveChanges();
            var products = new List<Product>()
            {
                new Product("Product 1", 1.03m),
                new Product("Product 2", 2.03m),
                new Product("Product 3", 3.03m)
            };
            context.Products.AddRange(products);

            context.SaveChanges();
        }

        private static void DeleteEventLogDatabase(IntegrationEventLogContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [IntegrationEventLog]");
            context.SaveChanges();
        }
    }
}
