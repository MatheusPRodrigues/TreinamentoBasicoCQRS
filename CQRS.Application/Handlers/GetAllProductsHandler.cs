using CQRS.Domain.DTOs;
using CQRS.Infraestructure.Context;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Application.Handlers
{
    public class GetAllProductsHandler
    {
        private readonly WriteContext _context;

        public GetAllProductsHandler(WriteContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO?>> HandleAsync()
        {
            var query = @"SELECT Name, Price FROM Products";
            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<ProductDTO>(query);
                return products;
            }
        }
    }
}
