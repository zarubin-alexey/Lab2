using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Lab2.RabbitMq
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;

        public RabbitMqService(IConfiguration configuration)
        {
            queueName = configuration.GetConnectionString("RabbitMq_WriteQueue");
            var uri = configuration.GetConnectionString("RabbitMq_Url");

            var factory = new ConnectionFactory() { Uri = new Uri(uri) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
        }

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", queueName, null, body);
        }
    }
}
