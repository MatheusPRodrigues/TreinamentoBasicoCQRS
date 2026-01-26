using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infraestructure.Context;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CQRS.Application.Events.Consumer
{
    public class ConsumeQueue : BackgroundService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IMongoCollection<Product> _collection;
        private readonly string _queue;

        public ConsumeQueue(
            IAbstractFactory<ConnectionFactory> connectionFactory,
            IAbstractFactory<IMongoDatabase> context
        )
        {
            _connectionFactory = connectionFactory.CreateConnection();
            _collection = context.CreateConnection().GetCollection<Product>("Products");
            _queue = "product_queue";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
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
                    await _collection.InsertOneAsync(persistItem);
                }
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(
                queue: _queue,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
