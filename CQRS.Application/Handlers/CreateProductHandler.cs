using CQRS.Application.Commands;
using CQRS.Infraestructure.Context;
using Dapper;

namespace CQRS.Application.Handlers
{
    public class CreateProductHandler
    {
        private readonly DapperContext _context;

        public CreateProductHandler(DapperContext context) => _context = context;   
        
        public async Task<int> HandleAsync(CreateProductCommand command)
        {
            var sql = @"INSERT INTO Products (Name, Price) VALUES (@Name, @Price);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _context.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(sql, new {command.Name, command.Price});

            return id; 
        }
    }
}
