using Lab2.Controllers;
using Lab2.Exceptions;
using Lab2.Models;
using Lab2.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Lab2.Orders
{
    public class OrdersListener : RabbitMqListener
    {
        private readonly IRabbitMqService rabbitService;
        private readonly IServiceScopeFactory factory;

        public OrdersListener(IConfiguration configuration, IRabbitMqService rabbitMqService, IServiceScopeFactory scopeFactory) : base(configuration)
        {
            rabbitService = rabbitMqService;
            factory = scopeFactory;
        }

        public override void OnMessageReceived(string message)
        {
            Order orderModel;
            try
            {
                orderModel = JsonConvert.DeserializeObject<Order>(message);
            }
            catch
            {
                throw new BadRequestException(ErrorCodes.WrongDataFormat, $"Failed to deserialize order data!");
            }

            using var scope = factory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EF_DataContext>();

            var order = OrdersApiController.Convert(orderModel);
            if (context.Orders.Any(p => p.id == order.id))
            {
                throw new BadRequestException(ErrorCodes.DataAlreadyExists, $"The order {order.id} is already added!");
            }

            try
            {
                context.Orders.Add(order);
                context.SaveChanges();

                var product = new Product() { Id = order.product_id, Count = order.count };

                rabbitService.SendMessage(product);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ErrorCodes.UnknownError, ex.Message);
            }
        }
    }
}
