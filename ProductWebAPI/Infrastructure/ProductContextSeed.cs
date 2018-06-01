using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using ProductWebAPI.Contexts;
using ProductWebAPI.Models;

namespace ProductWebAPI.Infrastructure
{
    public class ProductContextSeed
    {
        public async Task SeedAsync(ProductContext context, IHostingEnvironment env, IOptions<ProductSettings> settings)
        {
            var policy = CreatePolicy(nameof(ProductContextSeed));

            await policy.ExecuteAsync(async() => {

                using (context)
                {
                    context.Database.Migrate();

                    if (!context.Products.Any())
                    {
                        context.AddRange(GetPredifinedProducts());
                        await context.SaveChangesAsync();
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IList<Product> GetPredifinedProducts()
        {
            var products= new List<Product>()
            {
                new Product("Product 1", 1.03m),
                new Product("Product 2", 2.03m),
                new Product("Product 3", 3.03m)
            };
            return products;
        }

        private Policy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider:retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        Console.WriteLine(exception.Message + "retries: " + retry);
                    }
                );
        }
    }
}