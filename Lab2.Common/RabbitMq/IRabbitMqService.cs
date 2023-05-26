namespace Lab2.RabbitMq
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj);

        void SendMessage(string message);
    }
}
