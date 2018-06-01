using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderWebAPI.Contexts;
using OrderWebAPI.Models;
using Polly;

namespace OrderWebAPI.Infrastructure
{
    public class OrderContextSeed
    {
        public async Task SeedAsync(OrderContext context, IHostingEnvironment env, IOptions<OrderSettings> settings)
        {
            var policy = CreatePolicy(nameof(OrderContextSeed));

            await policy.ExecuteAsync(async() => {

                using (context)
                {
                    context.Database.Migrate();

                    if (!context.Orders.Any())
                    {
                        context.AddRange(GetPredifinedOrders());
                        await context.SaveChangesAsync();
                    }

                    if (!context.OrderItems.Any())
                    {
                        await context.SaveChangesAsync();
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IList<Order> GetPredifinedOrders()
        {
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
            return orders;
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