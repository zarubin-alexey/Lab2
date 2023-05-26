using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2.RabbitMq
{
    public abstract class RabbitMqListener : BackgroundService
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;

        public abstract void OnMessageReceived(string message);

        public RabbitMqListener(IConfiguration configuration)
        {
            queueName = configuration.GetConnectionString("RabbitMq_ReadQueue");
            var uri = configuration.GetConnectionString("RabbitMq_Url");

            var factory = new ConnectionFactory { Uri = new Uri(uri) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queueName, true, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                OnMessageReceived(content);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}
