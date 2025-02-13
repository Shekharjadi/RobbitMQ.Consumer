using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RobbitMQ.Consumer
{
    public static  class DirectExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);
            channel.QueueDeclare("demo-queue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);  // Corrected: Removed invalid parameter
            channel.QueueBind("demo-queue", "demo-direct-exchange", "account.init");
            channel.BasicQos(0,10, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received: {message}");
            };
            channel.BasicConsume("demo-queue", true, consumer);
            Console.ReadLine();
        }
    }
}
