using CQRS.Application.Commands;
using CQRS.Application.Events.Consumer;
using CQRS.Application.Events.Publisher;
using CQRS.Domain.DTOs;
using CQRS.Domain.Entities;
using CQRS.Infraestructure.Context;
using Dapper;
using MongoDB.Driver;

namespace CQRS.Application.Handlers
{
    public class CreateProductHandler
    {
        private readonly WriteContext _writeContext;
        private readonly PublishMessage _publishMessage;
        private readonly ConsumeQueue _consumeQueue;
        private readonly IMongoCollection<Product> _collection;

        public CreateProductHandler(
            WriteContext writeContext,
            PublishMessage publishMessage,
            ConsumeQueue consumeQueue,
            ReadContext readContext
            )
        {
            _writeContext = writeContext;
            _publishMessage = publishMessage;
            _consumeQueue = consumeQueue;
            _collection = readContext.GetDatabase()
                .GetCollection<Product>("Products");
        }

        public async Task<int> HandleAsync(CreateProductCommand command)
        {
            var sql = @"INSERT INTO Products (Name, Price) VALUES (@Name, @Price);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _writeContext.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(sql, new {command.Name, command.Price});

            var product = new ProductSendDTO
            {
                Id = id,
                Name = command.Name,
                Price = command.Price,
            };

            await _publishMessage.PublishAsync(product);
            await _consumeQueue.ConsumeAsync();

            var persistedItens = _consumeQueue.GetMessages();

            foreach (var itens in persistedItens)
            {
                var insertItem = new Product(
                    itens.Id,
                    itens.Name,
                    itens.Price
                );
                await _collection.InsertOneAsync(insertItem);
            }
            persistedItens.Clear();

            return id; 
        }
    }
}
