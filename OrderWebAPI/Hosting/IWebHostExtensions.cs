using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace OrderWebAPI.Hosting
{
    public static class IWebHostExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetService<TContext>();

                try
                {
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(new TimeSpan[]{
                            TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(20),
                            TimeSpan.FromSeconds(40),
                            TimeSpan.FromSeconds(80)
                        });

                    retry.Execute(() =>
                    {
                        context.Database
                            .Migrate();

                        seeder(context, services);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return webHost;
        }
    }
}