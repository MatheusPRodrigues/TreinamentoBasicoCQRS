using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CQRS.Application.Events.Consumer
{
    public class ConsumeQueue
    {
        private readonly IConnectionFactory _connectionFactory;

        public ConsumeQueue(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task ConsumeAsync(IMongoCollection<Product> collection)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "ProductQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount:  1,
                global: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var objectConsumed = JsonSerializer.Deserialize<ProductSendDTO>(message);
                if (objectConsumed is not null)
                {
                    var persistItem = new Product(
                        objectConsumed.Id,
                        objectConsumed.Name,
                        objectConsumed.Price
                    );
                    await collection.InsertOneAsync(persistItem);
                }
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(
                queue: "ProductQueue",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
