using CQRS.Application.Commands;
using CQRS.Application.Events.Publisher;
using CQRS.Domain.DTOs;
using CQRS.Infraestructure.Context;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CQRS.Application.Handlers
{
    public class CreateProductHandler
    {
        private readonly SqlConnection _writeContext;
        private readonly PublishMessage _publishMessage;

        public CreateProductHandler(
            IAbstractFactory<SqlConnection> writeContext,
            PublishMessage publishMessage
        )
        {
            _writeContext = writeContext.CreateConnection();
            _publishMessage = publishMessage;
        }

        public async Task<int> HandleAsync(CreateProductCommand command)
        {
            var sql = @"INSERT INTO Products (Name, Price) VALUES (@Name, @Price);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            await _writeContext.OpenAsync();
            var id = await _writeContext.QuerySingleAsync<int>(sql, new { command.Name, command.Price });
            await _writeContext.CloseAsync();

            var product = new ProductSendDTO
            {
                Id = id,
                Name = command.Name,
                Price = command.Price,
            };

            await _publishMessage.PublishAsync(product);

            return id;

        }
    }
}
