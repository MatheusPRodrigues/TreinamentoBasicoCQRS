using CQRS.Domain.DTOs;
using CQRS.Infraestructure.Context;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Application.Handlers
{
    public class GetProductByIdHandler
    {
        private readonly DapperContext _context;

        public GetProductByIdHandler(DapperContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO?> HandleAsync(int id)
        {
            var query = "SELECT Name, Price FROM Products WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<ProductDTO>(query, new { Id = id });

                return product;
            }
        }
    }
}
