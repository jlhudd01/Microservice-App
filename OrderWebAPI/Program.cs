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
using OrderWebAPI.Contexts;
using OrderWebAPI.Hosting;
using OrderWebAPI.Infrastructure;

namespace OrderWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<OrderContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var settings = services.GetService<IOptions<OrderSettings>>();

                    Task.Run(() => new OrderContextSeed()
                        .SeedAsync(context, env, settings))
                        .Wait();
                })
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
