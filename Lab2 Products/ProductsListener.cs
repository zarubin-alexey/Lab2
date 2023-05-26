using Lab2.Exceptions;
using Lab2.Models;
using Lab2.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Lab2.Products
{
    public class ProductsListener : RabbitMqListener
    {
        private readonly IServiceScopeFactory factory;

        public ProductsListener(IConfiguration configuration, IServiceScopeFactory scopeFactory) : base(configuration)
        {
            factory = scopeFactory;
        }

        public override void OnMessageReceived(string message)
        {
            Product product;
            try
            {
                product = JsonConvert.DeserializeObject<Product>(message);
            }
            catch
            {
                throw new BadRequestException(ErrorCodes.WrongDataFormat, $"Failed to deserialize product data!");
            }

            using var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EF_DataContext>();

            if (!context.Products.Any(p => p.id == product.Id))
            {
                throw new BadRequestException(ErrorCodes.EmptyResult, $"There is no product {product.Id}!");
            }

            var p = context.Products.Where(p => p.id == product.Id).FirstOrDefault();
            if (p.count > product.Count)
            {
                p.count -= product.Count;
            }
            else
            {
                throw new BadRequestException(ErrorCodes.NotEnoughtCount, $"It is not enough product {product.Id} ({p.count}) to sell {product.Count} items!");
            }

            context.SaveChanges();
        }
    }
}
