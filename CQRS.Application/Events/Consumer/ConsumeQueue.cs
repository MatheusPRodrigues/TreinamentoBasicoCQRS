using CQRS.Domain.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CQRS.Application.Events.Consumer
{
    public class ConsumeQueue
    {
        private readonly IConnectionFactory _connectionFactory;
        private List<ProductSendDTO> messages = new();

        public ConsumeQueue(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task ConsumeAsync()
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

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                var objectConsumed = JsonSerializer.Deserialize<ProductSendDTO>(message);
                if (objectConsumed is not null)
                {
                    messages.Add(objectConsumed);
                }
            };

            await channel.BasicConsumeAsync("ProductQueue", autoAck: true, consumer: consumer);
        }

        public List<ProductSendDTO> GetMessages()
        {
            return messages;
        }
    }
}
