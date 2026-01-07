using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CQRS.Application.Events.Publisher
{
    public class PublishMessage
    {
        private readonly IConnectionFactory _connectionFactory;

        public PublishMessage(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task PublishAsync(object message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "ProductQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "ProductQueue",
                body: body
            );
        }
    }
}
