using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductWebAPI.Contexts;
using ProductWebAPI.Hosting;
using ProductWebAPI.Infrastructure;

namespace ProductWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<ProductContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var settings = services.GetService<IOptions<ProductSettings>>();

                    Task.Run(() => new ProductContextSeed()
                        .SeedAsync(context, env, settings))
                        .Wait();
                })
                .MigrateDbContext<IntegrationEventLogContext>((_,__) => { })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) => 
                {
                    config.Sources.Clear();
                    config.AddJsonFile("settings.json");
                    config.AddEnvironmentVariables();
                })
                .Build();
    }
}
